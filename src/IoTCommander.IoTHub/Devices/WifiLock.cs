namespace IoTCommander.IoTHub.Devices;

public class WifiLock : IoTDeviceBase
{
    public WifiLock(string id, string name, string location, byte state = 1)
    {
        ID = id;
        Kind = "Lock";
        Name = name;
        Location = location;
        Properties = new()
        {
            { "locked", state }
        };
    }
}