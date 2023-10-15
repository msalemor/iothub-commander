using System.Text.Json;
using IoTCommander.IoTHub.Devices;

namespace IoTCommander.IoTHub.Services;

public class HomeDeviceServices
{
    private readonly List<IIoTDevice> devices;

    public HomeDeviceServices()
    {
        devices = new List<IIoTDevice>
        {
            new WifiLight("Light1", "Light", "Living room"),
            new WifiLight("Light2", "Light-Left", "Master bedroom"),
            new WifiLight("Light3", "Light-Right", "Master bedroom"),
            new WifiTv("Tv1", "Main Tv", "Living room"),
            new WifiTv("Tv2", "Master bedroom tv", "Master bedroom"),
            new WifiLock("Lock1", "Entry door lock", "Entrance"),
            new WifiGarage("Garage-South", "South Garage door", "Garage"),
            new WifiGarage("Garage-North", "North Garage door", "Garage"),
        };
    }

    public List<IIoTDevice> GetDevices
    {
        get
        {
            return devices;
        }
    }

    public string GetDevicesJson
    {
        get
        {
            return JsonSerializer.Serialize(devices.OrderBy(c => c.ID));
        }
    }

}