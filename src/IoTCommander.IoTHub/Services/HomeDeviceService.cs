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
            new WifiLight("1000","Pedestal Light","Living Room"),
            new WifiSwitch("1001","Corner Light","Living Room"),
            new WifiSwitch("1002","Right Light","Master Bedroom"),
            new WifiSwitch("1003","Left Light","Master Bedroom"),
            new WifiSwitch("1004","Office Light","Office"),
            new WifiThermostat("1005","Florida","Laundry Room"),
            new WifiThermostat("1006","Maine","Hallway"),
            new WifiTv("1007","Family TV","Family Room"),
            new WifiLight("1008","TV Light","Family Room"),
            new WifiTv("1009","Master TV","Master bedroom"),
        };
    }

    public List<IIoTDevice> GetDevices
    {
        get
        {
            return devices;
        }
    }
}