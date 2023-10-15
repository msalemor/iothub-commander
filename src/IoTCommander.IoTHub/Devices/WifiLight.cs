namespace IoTCommander.IoTHub.Devices;

public class WifiLight : IoTDeviceBase
{
    public WifiLight(string id, string name, string location, byte state = 0, byte brightness = 100, string color = "White")
    {
        ID = id;
        Kind = "Light";
        Name = name;
        Location = location;
        Properties["state"] = state;
        Properties["brightness"] = brightness;
        Properties["color"] = color;
    }
}