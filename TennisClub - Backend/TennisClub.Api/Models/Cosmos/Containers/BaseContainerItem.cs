using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace TennisClub.Api.Models.Cosmos.Containers;

public abstract class BaseContainerItem
{
    [JsonProperty(PropertyName = "id")]
    [JsonPropertyName("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();
}
