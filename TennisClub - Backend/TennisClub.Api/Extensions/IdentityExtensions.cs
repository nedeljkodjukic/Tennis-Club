using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.Cosmos;
using Mobsites.Cosmos.Identity;
using TennisClub.Api.Models.Cosmos.Containers;

namespace TennisClub.Api.Extensions;

public static class IdentityExtensions
{
    public static IdentityBuilder AddCosmosDbIdentityProvider(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddCosmosStorageProvider(options =>
        {
            options.ConnectionString = configuration["CosmosDb:ConnectionString"];// "{cosmos-connection-string}";
            options.CosmosClientOptions = new CosmosClientOptions
            {
                SerializerOptions = new CosmosSerializationOptions
                {
                    IgnoreNullValues = false
                }
            };
            options.DatabaseId = configuration["CosmosDb:DatabaseName"];
            options.ContainerProperties = new ContainerProperties
            {
                Id = "users",
                //PartitionKeyPath defaults to "/PartitionKey", which is what is desired for the default setup.
            };
        });

        return services.AddDefaultCosmosIdentity<ApplicationUser>(options =>
        {
            options.Password.RequiredLength = 8;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireDigit = true;
            options.User.RequireUniqueEmail = true;
        });
    }
}
