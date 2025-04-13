using Amazon;
using Microsoft.Extensions.DependencyInjection;
using QuorumMind.Infrastructure.RemoteConfiguration.Core;

namespace QuorumMind.Infrastructure.RemoteConfiguration.AWS.AppConfig;

public static class AwsRemoteProviderExtensions
{   
    public static IServiceCollection AddAwsAppConfigRemoteProvider(
        this IServiceCollection services,
        string appId,
        string envId,
        string profileId)
    {
        services.AddSingleton<IRemoteConfigProvider>(new AwsAppConfigRemoteConfigProvider(appId, envId, profileId));
        return services;
    }
    
    public static IServiceCollection AddAwsAppConfigRemoteProvider(
        this IServiceCollection services,
        string appId,
        string envId,
        string profileId,
        string accessKey,
        string secretKey,
        RegionEndpoint? region = null)
    {
        services.AddSingleton<IRemoteConfigProvider>(new AwsAppConfigRemoteConfigProvider(appId, envId, profileId, accessKey,secretKey,region));
        return services;
    }
}