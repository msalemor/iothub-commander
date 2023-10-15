namespace IoTCommander.IoTHub.Devices;

public class WifiThermostat : IoTDeviceBase
{
    public WifiThermostat(string id, string name, string location, byte state = 2, byte system = 1, byte temperature = 72)
    {
        ID = id;
        Kind = "Lock";
        Name = name;
        Location = location;
        Properties = new()
        {
            { "state", state },
            { "system", system},
            { "temperature", temperature}
        };
    }
}