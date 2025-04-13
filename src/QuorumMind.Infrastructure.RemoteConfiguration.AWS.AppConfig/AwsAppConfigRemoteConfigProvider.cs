using System.Text.Json;
using Amazon;
using Amazon.AppConfigData;
using Amazon.Runtime;
using QuorumMind.Infrastructure.RemoteConfiguration.Core;

namespace QuorumMind.Infrastructure.RemoteConfiguration.AWS.AppConfig;

public class AwsAppConfigRemoteConfigProvider : IRemoteConfigProvider
{
    private readonly IAmazonAppConfigData _client;
    private readonly string _appId, _envId, _profileId;
    private string? _token;

    public AwsAppConfigRemoteConfigProvider(string appId, string envId, string profileId)
    {
        _appId = appId;
        _envId = envId;
        _profileId = profileId;
        _client = new AmazonAppConfigDataClient();
    }
    
    public AwsAppConfigRemoteConfigProvider(
        string appId,
        string envId,
        string profileId,
        string accessKey,
        string secretKey,
        RegionEndpoint? region = null)
    {
        _appId = appId;
        _envId = envId;
        _profileId = profileId;

        var awsCredentials = new BasicAWSCredentials(accessKey, secretKey);
        var config = new AmazonAppConfigDataConfig();
        config.RegionEndpoint = region;

        _client = new AmazonAppConfigDataClient(awsCredentials, config);
    }


    public async Task<IDictionary<string, object>> GetConfigSectionsAsync(string scope, CancellationToken token)
    {
        if (_token == null)
        {
            var start = await _client.StartConfigurationSessionAsync(new()
            {
                ApplicationIdentifier = _appId,
                EnvironmentIdentifier = _envId,
                ConfigurationProfileIdentifier = _profileId
            }, token);
            _token = start.InitialConfigurationToken;
        }

        var response = await _client.GetLatestConfigurationAsync(new() { ConfigurationToken = _token }, token);
        _token = response.NextPollConfigurationToken;

        if (response.Configuration.Length == 0)
            return new Dictionary<string, object>();
        
        using var reader = new StreamReader(response.Configuration);
        var json = await reader.ReadToEndAsync(token);

        return JsonSerializer.Deserialize<Dictionary<string, object>>(json)
               ?? new Dictionary<string, object>();
    }
}
