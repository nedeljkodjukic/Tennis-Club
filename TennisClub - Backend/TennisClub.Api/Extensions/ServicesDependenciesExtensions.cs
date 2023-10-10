using Microsoft.Azure.Cosmos;
using TennisClub.Api.Services;

namespace TennisClub.Api.Extensions;

public static class ServicesDependenciesExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        var cosmosClient = new CosmosClient(
            accountEndpoint: configuration["CosmosDb:Endpoint"],
            authKeyOrResourceToken: configuration["CosmosDb:PrimaryKey"]);

        return services
            .AddScoped<IdentityService>()
            .AddSingleton<ImageStoreService>()
            .AddSingleton<BaseCosmosRepository>()
            .AddSingleton<PlayerService>()
            .AddSingleton<TournamentService>()
            .AddSingleton<RankingService>()
            .AddSingleton<MatchService>()
            .AddSingleton(cosmosClient);
    }
}
