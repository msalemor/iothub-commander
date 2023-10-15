namespace IoTCommander.IoTHub.Devices;

public class WifiSwitch : IoTDeviceBase
{
    public WifiSwitch(string id, string name, string location, byte state = 0)
    {
        ID = id;
        Kind = "Switch";
        Name = name;
        Location = location;
        Properties = new()
        {
            { "state", state }
        };
    }
}
