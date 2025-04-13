using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace QuorumMind.Infrastructure.RemoteConfiguration.Core;

public class RemoteConfigBackgroundUpdater : BackgroundService
{
    private readonly IRemoteConfigProvider _provider;
    private readonly RemoteConfigApplier _applier;
    private readonly string _scope;
    private readonly TimeSpan _interval;

    public RemoteConfigBackgroundUpdater(IRemoteConfigProvider provider, 
        RemoteConfigApplier applier, 
        string scope,
        TimeSpan interval)
    {
        _provider = provider;
        _applier = applier;
        _scope = scope;
        _interval = interval;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var sections = await _provider.GetConfigSectionsAsync(_scope, stoppingToken);
            _applier.Apply(sections);
            await Task.Delay(_interval, stoppingToken);
        }
    }
}
