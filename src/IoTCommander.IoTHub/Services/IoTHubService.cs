using System.Configuration;
using IoTCommander.Common.Services;
using Microsoft.Azure.Devices;

namespace IoTCommander.IoTHub.Services;

public class IoTHubService
{
    private readonly string registryConnStr;
    private readonly string serviceConnStr;

    public IoTHubService(AppSettingsService appSettings)
    {
        registryConnStr = appSettings.IoTRegistryConnStr;
        serviceConnStr = appSettings.IoTDeviceConnStr;
    }

    public async Task<Device?> CreateDeviceAsync(string deviceId)
    {
        try
        {
            using var registryManager = RegistryManager.CreateFromConnectionString(registryConnStr);
            var device = new Device(deviceId);
            device = await registryManager.AddDeviceAsync(device);
            return device;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return null;
    }

    public async Task<IEnumerable<Device>> ListDevicesAsync()
    {
        using var registryManager = RegistryManager.CreateFromConnectionString(registryConnStr);
        var devices = await registryManager.GetDevicesAsync(100);
        // var reader = registryManager.CreateQuery("SELECT * FROM devices", 100);
        // while (reader.HasMoreResults)
        // {
        //     var page = await reader.
        //     foreach (var twin in page)
        //     {
        //         var json = twin.ToJson();
        //     }
        // }
        return devices;
    }

    public async Task SendCommandAsync(string deviceId, string commandName, string payload)
    {
        using var serviceClient = ServiceClient.CreateFromConnectionString(serviceConnStr);
        var methodInvocation = new CloudToDeviceMethod(commandName) { ResponseTimeout = TimeSpan.FromSeconds(30) };
        methodInvocation.SetPayloadJson(payload);
        var response = await serviceClient.InvokeDeviceMethodAsync(deviceId, methodInvocation);
        if (response.Status == 200)
        {
            var json = response.GetPayloadAsJson();
            Console.WriteLine(json);
        }
        //var responsePayload = Encoding.UTF8.GetString(response.GetPayloadAsBytes());
        // Handle the response payload here
    }
}