namespace QuorumMind.Infrastructure.RemoteConfiguration.Core;

public interface IRemoteConfigUpdater
{
    void Apply(Dictionary<string, string> configValues);
}