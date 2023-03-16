// file:///home/pizzaboxer/Documents/Projects/RobloxSetArchive/dotnet-vue/RobloxSetArchive.Api/Data/Entities/AssetSet.cs {"mtime":1671135918716,"ctime":1670195095386,"size":546,"etag":"39pder5fdhj","orphaned":false,"typeId":""}
namespace RobloxSetArchive.Api.Data.Entities;

public class AssetSet
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public long ImageAssetId { get; set; }
    public int ImageAssetUpdated { get; set; }
    public string CreatorName { get; set; } = null!;
    public int CreatorId { get; set; }
    public int SubscriberCount { get; set; }


    // public virtual List<Asset> Assets { get; set; }
    // public virtual List<Subscriber> Subscribers { get; set; }
}