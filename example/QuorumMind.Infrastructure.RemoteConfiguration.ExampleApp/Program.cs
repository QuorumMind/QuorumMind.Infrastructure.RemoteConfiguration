using Amazon;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using QuorumMind.Infrastructure.RemoteConfiguration.AWS.AppConfig;
using QuorumMind.Infrastructure.RemoteConfiguration.Consul;
using QuorumMind.Infrastructure.RemoteConfiguration.Core;
using QuorumMind.Infrastructure.RemoteConfiguration.ExampleApp.App.Models;
using QuorumMind.Infrastructure.RemoteConfiguration.GCP.Firestore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

SetupConsulRemoteConfigProvider();
//SetupFirebaseRemoteConfigProvider();
//SetupAwsRemoteAppConfigProvider();


//EXAMPLE OF USAGE
builder.Services.Configure<EmailConfig>(builder.Configuration.GetSection("EmailSettings"));



var app = builder.Build();
app.MapControllers();

app.UseHttpsRedirection();
app.Run();



void SetupConsulRemoteConfigProvider()
{
    builder.Services
        .AddConsulRemoteProvider("<<CONSUL ADDRESS>>")
        .AddRemoteConfigurationSupport(builder.Configuration);;
}

void SetupAwsRemoteAppConfigProvider()
{
    string accessKey = "{PLACEHOLDER}";
    string secretKey = "{PLACEHOLDER}";

    string appId = "";
    string envId = "";
    string profileId = "";

    builder.Services
        .AddAwsAppConfigRemoteProvider(appId, envId, profileId, accessKey, secretKey,RegionEndpoint.USEast2)
        .AddRemoteConfigurationSupport(builder.Configuration);;
}

void SetupFirebaseRemoteConfigProvider()
{
    var credential = GoogleCredential.FromFile("./{PATH TO SERVICE ACCOUNT FILE}");
    var fireStoreBuilder = new FirestoreClientBuilder
    {
        Credential = credential,
    };
    var firestoreDB = FirestoreDb.Create("<<PROJECTID PLACEHOLDER>>", fireStoreBuilder.Build());
    builder.Services
        .AddFirestoreRemoteProvider(firestoreDB)
        .AddRemoteConfigurationSupport(builder.Configuration);;
}
