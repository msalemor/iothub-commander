namespace IoTCommander.IoTHub.Devices;

public class WifiTv : IoTDeviceBase
{
    public WifiTv(string id, string name, string location, byte state = 0, byte volume = 0, byte channel = 1, byte input = 1)
    {
        ID = id;
        Kind = "TV";
        Name = name;
        Location = location;
        Properties = new()
        {
            { "state", state },
            { "volume", volume },
            { "channel", channel },
            { "input", input }
        };
    }
}
