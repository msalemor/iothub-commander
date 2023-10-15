namespace IoTCommander.IoTHub.Devices;

public class WifiGarage : IoTDeviceBase
{
    public WifiGarage(string id, string name, string location, byte open = 0, byte state = 0)
    {
        ID = id;
        Kind = "Garage";
        Name = name;
        Location = location;
        Properties = new()
        {
            { "open", open },
            { "light", state }
        };
    }
}