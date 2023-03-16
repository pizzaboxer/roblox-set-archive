using RobloxSetArchive.Api.Data.Entities;

namespace RobloxSetArchive.Api.Models;

public class UserResponseModel : User
{
    public IEnumerable<AssetSet> Owned { get; set; } = Enumerable.Empty<AssetSet>();
    public IEnumerable<AssetSet> Subscribed { get; set; } = Enumerable.Empty<AssetSet>();
}