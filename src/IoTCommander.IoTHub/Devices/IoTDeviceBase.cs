using System.Text.Json.Serialization;

namespace IoTCommander.IoTHub.Devices;

public abstract class IoTDeviceBase : IIoTDevice
{
    [JsonPropertyName("id")]
    public string ID { get; set; } = null!;
    [JsonPropertyName("kind")]
    public string Kind { get; set; } = "Light";
    [JsonPropertyName("name")]
    public string Name { get; set; } = "Light 1";
    [JsonPropertyName("location")]
    public string Location { get; set; } = "Living Room";
    [JsonPropertyName("properties")]
    public Dictionary<string, object> Properties { get; set; } = new() {
        { "state", 0 },
        { "brightness", 100 },
        { "color", "white" }
    };
}
