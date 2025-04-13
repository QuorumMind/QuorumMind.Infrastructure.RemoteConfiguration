using System.Text;
using System.Text.Json;
using Consul;
using QuorumMind.Infrastructure.RemoteConfiguration.Core;

namespace QuorumMind.Infrastructure.RemoteConfiguration.Consul;

using Consul;

public class ConsulRemoteConfigProvider : IRemoteConfigProvider
{
    private readonly ConsulClient _client;
    private readonly string _prefix;

    public ConsulRemoteConfigProvider(string consulUrl)
    {
        _client = new ConsulClient(cfg => cfg.Address = new Uri(consulUrl));
    }

    public async Task<IDictionary<string, object>> GetConfigSectionsAsync(string scope, CancellationToken token)
    {
        var result = await _client.KV.List(scope + "/", token);
        return result.Response?
                   .Where(kv => kv.Value != null)
                   .ToDictionary(
                       kv => kv.Key.Replace($"{scope}/", ""),
                       kv =>
                       {
                           var raw = Encoding.UTF8.GetString(kv.Value);
                           return (object)JsonSerializer.Deserialize<object>(raw)!;
                       }) 
               ?? new Dictionary<string, object>();

    }
}
