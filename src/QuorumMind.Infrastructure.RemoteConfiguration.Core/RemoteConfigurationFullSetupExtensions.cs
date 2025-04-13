using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace QuorumMind.Infrastructure.RemoteConfiguration.Core;

public static class RemoteConfigurationFullSetupExtensions
{
    public static IServiceCollection AddRemoteConfigurationSupport(
        this IServiceCollection services,
        IConfiguration configuration,
        string? scopeOverride = null,
        TimeSpan? intervalOverride = null)
    {
        services.AddSingleton<RemoteConfigApplier>();

        services.AddHostedService(sp =>
        {
            var provider = sp.GetRequiredService<IRemoteConfigProvider>();
            var applier = sp.GetRequiredService<RemoteConfigApplier>();
            var scope = scopeOverride ?? configuration["RemoteConfig:Scope"]!;
            var interval = intervalOverride ??
                           TimeSpan.FromSeconds(int.TryParse(configuration["RemoteConfig:RefreshSeconds"], out var s) ? s : 30);

            return new RemoteConfigBackgroundUpdater(provider, applier, scope, interval);
        });

        return services;
    }
}
