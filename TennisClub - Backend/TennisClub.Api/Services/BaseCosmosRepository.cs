using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using TennisClub.Api.Models.Cosmos.Containers;

namespace TennisClub.Api.Services;

public class BaseCosmosRepository
{
    private readonly CosmosClient _cosmosClient;
    private readonly Database _database;
    public readonly Dictionary<Type, Container> Containers;

    public BaseCosmosRepository(CosmosClient cosmosClient, IConfiguration configuration)
    { 
        _cosmosClient = cosmosClient;
        _database = _cosmosClient.GetDatabase(configuration["CosmosDb:DatabaseName"]);
        Containers = new()
        {
            { typeof(Match), _database.GetContainer("matches") },
            { typeof(Player), _database.GetContainer("players") },
            { typeof(Tournament), _database.GetContainer("tournaments") },
            { typeof(WeeklyRank), _database.GetContainer("weeklyRanks") }
        };
    }

    public async Task<TItem> GetAsync<TItem>(string id, CancellationToken cancellationToken = default)
        where TItem : BaseContainerItem
        => await Containers[typeof(TItem)].ReadItemAsync<TItem>(id: id, partitionKey: new PartitionKey(id), cancellationToken: cancellationToken);

    public async Task<IEnumerable<TItem>> GetAllAsync<TItem>(CancellationToken cancellationToken = default)
        where TItem : BaseContainerItem
    {
        var items = new List<TItem>();

        using var feedIterator = Containers[typeof(TItem)].GetItemLinqQueryable<TItem>().Where(x => true).ToFeedIterator();

        while (feedIterator.HasMoreResults)
        {
            var response = await feedIterator.ReadNextAsync(cancellationToken);

            items.AddRange(response);
        }

        return items;
    }

    public async Task<IEnumerable<TOutput>> GetItemsFromQueryAsync<TItem, TOutput>(string queryText, Dictionary<string, object> queryParams = default, CancellationToken cancellationToken = default)
        where TItem : BaseContainerItem
    {
        var outputItems = new List<TOutput>();

        var queryDefinition = new QueryDefinition(queryText);
        if (queryParams != null)
            foreach (var item in queryParams)
                queryDefinition = queryDefinition.WithParameter(item.Key, item.Value);

        using var feedIterator = Containers[typeof(TItem)].GetItemQueryIterator<TOutput>(queryDefinition);

        while (feedIterator.HasMoreResults)
        {
            var response = await feedIterator.ReadNextAsync(cancellationToken);

            outputItems.AddRange(response);
        }

        return outputItems;
    }

    public Task<IEnumerable<TItem>> GetItemsFromQueryAsync<TItem>(string queryText, Dictionary<string, object> queryParams = default, CancellationToken cancellationToken = default)
        where TItem : BaseContainerItem
        => GetItemsFromQueryAsync<TItem, TItem>(queryText, queryParams, cancellationToken);

    public async Task<string> InsertAsync<TItem>(TItem record, CancellationToken cancellationToken = default)
        where TItem : BaseContainerItem
    {
        var response = await Containers[typeof(TItem)].CreateItemAsync<TItem>(item: record, partitionKey: new PartitionKey(record.Id), cancellationToken: cancellationToken);

        return response.Resource.Id;
    }


    public Task UpdateAsync<TItem>(string id, TItem record, CancellationToken cancellationToken = default)
        where TItem : BaseContainerItem
        => Containers[typeof(TItem)].ReplaceItemAsync<TItem>(item: record, id: id, partitionKey: new PartitionKey(id), cancellationToken: cancellationToken);

    public Task RemoveAsync<TItem>(string id, CancellationToken cancellationToken = default)
        where TItem : BaseContainerItem
        => Containers[typeof(TItem)].DeleteItemAsync<TItem>(id: id, partitionKey: new PartitionKey(id), cancellationToken: cancellationToken);
}
