using Microsoft.Extensions.DependencyInjection;
using QuorumMind.Infrastructure.RemoteConfiguration.Core;

namespace QuorumMind.Infrastructure.RemoteConfiguration.Consul;

public static class ConsulRemoteProviderExtensions
{
    public static IServiceCollection AddConsulRemoteProvider(
        this IServiceCollection services,
        string consulUrl)
    {
        services.AddSingleton<IRemoteConfigProvider>(
            new ConsulRemoteConfigProvider(consulUrl));

        return services;
    }
}