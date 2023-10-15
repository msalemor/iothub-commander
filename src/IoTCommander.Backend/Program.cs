using System.Text.Json;
using IoTCommander.Common.Models;
using IoTCommander.Common.Services;
using IoTCommander.IoTHub.Devices;
using IoTCommander.IoTHub.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Devices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(opts => opts.AddDefaultPolicy(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

// Configure the simulated devices
var settingsService = new AppSettingsService();
var ctx = new CancellationTokenSource();
var iotHubService = new IoTHubService(settingsService);
var desiredDevices = new HomeDeviceServices().GetDevices;
var actualDevices = (await iotHubService.ListDevicesAsync()).ToList();
// Compare the list of desired devices vs the list of connected in IoT Hub
desiredDevices = desiredDevices.Where(c => !actualDevices.Any(d => d.Id == c.ID && d.ConnectionState == DeviceConnectionState.Connected)).ToList();
// For each desired device create a simulated devices
builder.Services.AddSingleton(settingsService);
builder.Services.AddSingleton(iotHubService);

var app = builder.Build();

// Start the simulated devices
List<Task> tasks = new();
foreach (var device in desiredDevices)
{
    var simulatedDevice = new SimulatedDevice(device, iotHubService, ctx, settingsService);
    tasks.Add(simulatedDevice.RunAsync());
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
// app.UseAuthorization();
// app.MapControllers();

var group = app.MapGroup("/api/v1/iothub");

//app.UseAuthorization();
//app.MapControllers();
group.MapGet("/devices", () =>
{
    return Results.Ok(desiredDevices);
});

group.MapGet("/devices/{deviceId}", (string deviceId) =>
{
    if (string.IsNullOrEmpty(deviceId))
    {
        return Results.BadRequest(new { message = "deviceId is required" });
    }
    return Results.Ok(desiredDevices.Where(c => c.ID == deviceId).FirstOrDefault());
});

group.MapPost("/devices", () =>
{
    return Results.Ok();
});

group.MapPut("/devices", async ([FromBody] CommandsRequest request, IoTHubService service) =>
{
    if (string.IsNullOrEmpty(request.deviceId) || request.commands == null || request.commands.Count == 0)
    {
        return Results.BadRequest(new { message = "deviceId and commands are required" });
    }
    var serializedJson = await service.SendCommandAsync(request.deviceId, "SetProperties", JsonSerializer.Serialize(request));
    if (string.IsNullOrEmpty(serializedJson))
    {
        return Results.BadRequest(new { message = "Unable to process commands" });
    }
    return Results.Ok(JsonSerializer.Deserialize<object>(serializedJson));
});

app.Run();
