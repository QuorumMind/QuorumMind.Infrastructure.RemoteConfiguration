using Google.Cloud.Firestore;
using Microsoft.Extensions.DependencyInjection;
using QuorumMind.Infrastructure.RemoteConfiguration.Core;

namespace QuorumMind.Infrastructure.RemoteConfiguration.GCP.Firestore;

public static class GCPRemoteProviderExtensions
{
    public static IServiceCollection AddFirestoreRemoteProvider(
        this IServiceCollection services,
        FirestoreDb db,
        string collection = "remote-configs")
    {
        services.AddSingleton<IRemoteConfigProvider>(new FirestoreRemoteConfigProvider(db, collection));

        return services;
    }
}