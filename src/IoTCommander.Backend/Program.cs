var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseAuthorization();
// app.MapControllers();

var group = app.MapGroup("/api/v1/iothub");

//app.UseAuthorization();
//app.MapControllers();
group.MapGet("/devices", () =>
{
    return Results.Ok();
});

group.MapGet("/devices/{deviceId}", () =>
{
    return Results.Ok();
});

group.MapPost("/devices", () =>
{
    return Results.Ok();
});

group.MapPut("/devices", () =>
{
    return Results.Ok();
});

app.Run();
