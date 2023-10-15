namespace IoTCommander.Common.Models;
public record CommandsRequest(string deviceId, Dictionary<string, object> commands);