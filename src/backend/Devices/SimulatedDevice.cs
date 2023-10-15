using System.Text;
using System.Text.Json;
using backend.Services;
using Microsoft.Azure.Devices.Client;
using Models;

namespace backend.Devices;

public abstract class IoTDeviceBase
{
    public required string ID { get; set; }
    public string DeviceType { get; set; }
    public required string Name { get; set; }
    public required string Location { get; set; }
    public byte IsAlive { get; set; } = 1; // 0=Dead, 1=Alive
    public Dictionary<string, object> Settings { get; set; } = new();

}


public class SimulatedDevice
{
    private TimeSpan s_telemetryInterval = TimeSpan.FromSeconds(60);
    public AppSettings Settings = new();

    private readonly IIotDevice device;
    public CancellationTokenSource CTS = new();
    public IotHubService Service;
    public SimulatedDevice(IIotDevice device, IotHubService service, CancellationTokenSource cts, AppSettings settings)
    {
        this.device = device;
        this.Service = service;
        this.CTS = cts;
        this.Settings = settings;
    }

    public async Task RunAsync()
    {
        await Service.CreateDeviceAsync(device.ID);
        var transportType = TransportType.Mqtt;
        var finalConnStr = Settings.IoTDeviceConnStr.Replace("<DeviceId>", device.ID);
        using var deviceClient = DeviceClient.CreateFromConnectionString(finalConnStr, transportType);
        await deviceClient.SetMethodDefaultHandlerAsync(DirectMethodCallback, null);
        await SendDeviceToCloudMessagesAsync(deviceClient, CTS.Token);
    }
    private async Task<MethodResponse> DirectMethodCallback(MethodRequest methodRequest, object userContext)
    {
        Console.WriteLine($"Received direct method [{methodRequest.Name}] with payload [{methodRequest.DataAsJson}].");
        string messageBody = "";
        byte[] bytes = null!;
        switch (methodRequest.Name)
        {
            case "SetTelemetryInterval":
                try
                {
                    int telemetryIntervalSeconds = JsonSerializer.Deserialize<int>(methodRequest.DataAsJson);
                    s_telemetryInterval = TimeSpan.FromSeconds(telemetryIntervalSeconds);
                    Console.WriteLine($"Setting the telemetry interval to {s_telemetryInterval}.");
                    return await Task.FromResult(new MethodResponse(200));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed ot parse the payload for direct method {methodRequest.Name} due to {ex}");
                    break;
                }
            case "GetStatus":
                messageBody = JsonSerializer.Serialize(device);
                bytes = Encoding.ASCII.GetBytes(messageBody);
                return await Task.FromResult(new MethodResponse(bytes, 200));
            case "SetProperties":
                try
                {
                    var request = JsonSerializer.Deserialize<CommandsRequest>(methodRequest.DataAsJson);
                    if (request is not null)
                        foreach (var command in request.commands.Keys)
                        {
                            try
                            {
                                device.Settings[command] = request.commands[command];
                                Console.WriteLine($"Setting property {command} to {request.commands[command]}");
                            }
                            catch
                            {
                                Console.WriteLine($"Failed to set property {command} to {request.commands[command]}");
                            }
                        }
                    messageBody = JsonSerializer.Serialize(device);
                    bytes = Encoding.ASCII.GetBytes(messageBody);
                    return await Task.FromResult(new MethodResponse(bytes, 200));
                    //await SendMessageAsync(deviceClient, device, ct);
                    //return await Task.FromResult(new MethodResponse(200));

                }
                catch (Exception ex)
                {

                    Console.WriteLine($"Failed ot parse the payload for direct method {methodRequest.Name} due to {ex}");
                    break;
                }
        }

        return await Task.FromResult(new MethodResponse(400));
    }

    // Async method to send simulated telemetry
    private async Task SendDeviceToCloudMessagesAsync(DeviceClient deviceClient, CancellationToken ct)
    {
        // Initial telemetry values
        double minTemperature = 20;
        double minHumidity = 60;
        var rand = new Random();

        try
        {
            while (!ct.IsCancellationRequested)
            {
                double currentTemperature = minTemperature + rand.NextDouble() * 15;
                double currentHumidity = minHumidity + rand.NextDouble() * 20;

                // Add a custom application property to the message.
                // An IoT hub can filter on these properties without access to the message body.
                //message.Properties.Add("temperatureAlert", (currentTemperature > 30) ? "true" : "false");




                // Send the telemetry message
                await SendMessageAsync(deviceClient, device, ct);

                await Task.Delay(s_telemetryInterval, ct);
            }
        }
        catch (TaskCanceledException) { } // ct was signaled
    }

    private async Task SendMessageAsync(DeviceClient deviceClient, IIotDevice device, CancellationToken ct)
    {

        string messageBody = JsonSerializer.Serialize(device);
        using var message = new Message(Encoding.ASCII.GetBytes(messageBody))
        {
            ContentType = "application/json",
            ContentEncoding = "utf-8",
        };
        // message.Properties.Add("kind", device.Kind);
        // message.Properties.Add("location", device.Location);
        // foreach (var prop in device.Settings.Keys)
        // {
        //     message.Properties.Add(prop, device.Settings[prop].ToString());
        // }

        await deviceClient.SendEventAsync(message, ct);
        Console.WriteLine($"{DateTime.Now} > Sending message: {messageBody}");
    }

}
