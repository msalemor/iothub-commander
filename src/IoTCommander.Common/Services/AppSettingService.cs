using dotenv.net;

namespace IoTCommander.Common.Services;

public class AppSettingsService
{
    public string GptDeploymentName { get; private set; }
    public string Endpoint { get; private set; }
    public string ApiKey { get; private set; }
    public string IoTRegistryConnStr { get; private set; }
    public string IoTServiceConnStr { get; private set; }
    public string IoTDeviceConnStr { get; private set; }

    public AppSettingsService()
    {
        // Read environment variables
        DotEnv.Load();
        GptDeploymentName = Environment.GetEnvironmentVariable("GPT_DEPLOYMENT_NAME") ?? "gpt";
        Endpoint = Environment.GetEnvironmentVariable("GPT_ENDPOINT") ?? "";
        ApiKey = Environment.GetEnvironmentVariable("GPT_API_KEY") ?? "";
        IoTRegistryConnStr = Environment.GetEnvironmentVariable("IOT_REGISTRY_CONN_STR") ?? "";
        IoTServiceConnStr = Environment.GetEnvironmentVariable("IOT_SERVICE_CONN_STR") ?? "";
        IoTDeviceConnStr = Environment.GetEnvironmentVariable("IOT_DEVICE_CONN_STR") ?? "";

        if (string.IsNullOrEmpty(GptDeploymentName) || string.IsNullOrEmpty(Endpoint) || string.IsNullOrEmpty(ApiKey) || string.IsNullOrEmpty(IoTRegistryConnStr))
        {
            Console.WriteLine("Missing configuration. Please set GPT_DEPLOYMENT_NAME, GPT_ENDPOINT, IOT_SERVICE_CONN_STR, and GPT_API_KEY environment variables.");
            Environment.Exit(1);
        }
    }
}