// See https://aka.ms/new-console-template for more information
using IoTCommander.IoTHub.Services;

var devices = new HomeDeviceServices().GetDevices;

foreach (var device in devices)
    Console.WriteLine(device.ID);