using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;

namespace QuorumMind.Infrastructure.RemoteConfiguration.Core;

public class RemoteConfigApplier
{
    private readonly IConfigurationRoot _configurationRoot;

    public RemoteConfigApplier(IConfiguration config)
    {
        _configurationRoot = (IConfigurationRoot)config;
    }

    public void Apply(IDictionary<string, object> sections)
    {
        var memoryData = new Dictionary<string, string>();
        foreach (var (section, value) in sections)
        {
            using var doc = JsonDocument.Parse(JsonSerializer.Serialize(value));
            Flatten(section, doc.RootElement, memoryData);
        }

        var newMemorySource = new MemoryConfigurationSource { InitialData = memoryData };
        var newProvider = new MemoryConfigurationProvider(newMemorySource);
        newProvider.Load();

        (_configurationRoot as IConfigurationBuilder)!.Add(newMemorySource); 
    }


    private void Flatten(string prefix, JsonElement element, Dictionary<string, string> result)
    {
        foreach (var prop in element.EnumerateObject())
        {
            var key = $"{prefix}:{prop.Name}";
            if (prop.Value.ValueKind == JsonValueKind.Object)
                Flatten(key, prop.Value, result);
            else
                result[key] = prop.Value.ToString() ?? "";
        }
    }
}
