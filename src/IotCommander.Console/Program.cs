// See https://aka.ms/new-console-template for more information
using IoTCommander.Common.Services;
using IoTCommander.IoTHub.Devices;
using IoTCommander.IoTHub.Services;
using Microsoft.Azure.Devices;

var desiredDevices = new HomeDeviceServices().GetDevices;

foreach (var device in desiredDevices)
    Console.WriteLine(device.ID);

var settings = new AppSettingsService();
var ctx = new CancellationTokenSource();
var iotHubService = new IoTHubService(settings);

Console.CancelKeyPress += async (sender, cts) =>
{
    Console.WriteLine("Stopping...");
    ctx.Cancel();
    await Task.Delay(1000);
    Environment.Exit(0);
};

var actualDevices = (await iotHubService.ListDevicesAsync()).ToList();

// Compare the list of desired devices vs the list of connected in IoT Hub
desiredDevices = desiredDevices.Where(c => !actualDevices.Any(d => d.Id == c.ID && d.ConnectionState == DeviceConnectionState.Connected)).ToList();

// For each desired device create a simulated devices
List<Task> tasks = new();
foreach (var device in desiredDevices)
{
    var simulatedDevice = new SimulatedDevice(device, iotHubService, ctx, settings);
    tasks.Add(simulatedDevice.RunAsync());
}

// Wait for all tasks to be completed
// Go to the IoT Hub in the Azure portal, select an active device, and send the following direct method:
// Method: SetProperties
// Payload: {"commands":{"state":1,"brightness":50,"color":"Purple"}}
// Expected Output:
// {
//     "status": 200,
//     "payload": {
//         "ID": "1000",
//         "Kind": "Light",
//         "Name": "Pedestal Light",
//         "Location": "Living Room",
//         "Properties": {
//             "state": 1,
//             "brightness": 50,
//             "color": "Purple"
//         }
//     }
// }
await Task.WhenAll(tasks);