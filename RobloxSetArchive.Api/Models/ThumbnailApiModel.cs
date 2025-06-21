using System.Text.Json.Serialization;

namespace RobloxSetArchive.Api.Models;

public class ThumbnailApiModel
{
    public List<ThumbnailApiModelContents> Data { get; set; } = new();
}

public class ThumbnailApiModelContents
{
    [JsonPropertyName("targetId")]
    public int TargetId { get; set; }

    [JsonPropertyName("state")]
    public string State { get; set; } = "";

    [JsonPropertyName("imageUrl")]
    public string ImageUrl { get; set; } = "";

    [JsonPropertyName("version")]
    public string Version { get; set; } = "";
}
