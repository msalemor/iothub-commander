namespace IoTCommander.IoTHub.Devices;

public interface IIoTDevice
{
    string ID { get; set; }
    string Kind { get; set; }
    string Name { get; set; }
    string Location { get; set; }
    Dictionary<string, object> Properties { get; set; }
}
