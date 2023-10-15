namespace IoTCommander.IoTHub.Devices;

public class WifiAlarm : IoTDeviceBase
{
    public WifiAlarm(string id, string name, string location, byte state = 0)
    {
        ID = id;
        Kind = "Alarm";
        Name = name;
        Location = location;
        Properties = new()
        {
            { "state", state },
            { "ts", "" }
        };
    }
}