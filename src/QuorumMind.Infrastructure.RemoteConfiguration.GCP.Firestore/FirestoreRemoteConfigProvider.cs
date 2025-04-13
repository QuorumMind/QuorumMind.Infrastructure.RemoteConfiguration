using QuorumMind.Infrastructure.RemoteConfiguration.Core;

namespace QuorumMind.Infrastructure.RemoteConfiguration.GCP.Firestore;

using Google.Cloud.Firestore;

public class FirestoreRemoteConfigProvider : IRemoteConfigProvider
{
    private readonly FirestoreDb _db;
    private readonly string _collection;

    public FirestoreRemoteConfigProvider(FirestoreDb db, string collection = "remote-configs")
    {
        _db = db;
        _collection = collection;
    }

    public async Task<IDictionary<string, object>> GetConfigSectionsAsync(string scope, CancellationToken token)
    {
        var doc = await _db.Collection(_collection).Document(scope).GetSnapshotAsync(token);
        if (!doc.Exists) return new Dictionary<string, object>();
        return doc.ToDictionary();
    }
}