using System.Text.Json.Serialization;

namespace IoTCommander.IoTHub.Devices;

public interface IIoTDevice
{
    [JsonPropertyName("id")]
    string ID { get; set; }
    [JsonPropertyName("kind")]
    string Kind { get; set; }
    [JsonPropertyName("name")]
    string Name { get; set; }
    [JsonPropertyName("location")]
    string Location { get; set; }
    [JsonPropertyName("properties")]
    Dictionary<string, object> Properties { get; set; }
}
