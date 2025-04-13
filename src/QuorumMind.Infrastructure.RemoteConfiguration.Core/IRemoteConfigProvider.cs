namespace QuorumMind.Infrastructure.RemoteConfiguration.Core;

public interface IRemoteConfigProvider
{
    Task<IDictionary<string, object>> GetConfigSectionsAsync(string configScope, CancellationToken cancellationToken);
}