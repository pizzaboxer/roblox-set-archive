namespace RobloxSetArchive.Api.Data.Entities;

public class Asset
{
    public int Id { get; set; }
    public int AssetSetId { get; set; }
    public int AssetId { get; set; }
    public long AssetVersionId { get; set; }
    public string AssetName { get; set; } = null!;
    public string CreatorName {get; set;} = null!;
}