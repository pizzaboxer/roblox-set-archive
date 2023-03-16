// file:///home/pizzaboxer/Documents/Projects/RobloxSetArchive/dotnet-vue/RobloxSetArchive.Api/XmlBuilder.cs {"mtime":1671324121491,"ctime":1671322102482,"size":1795,"etag":"39pmk0q191qs","orphaned":false,"typeId":""}
// no, we're not using XElement. lol

using RobloxSetArchive.Api.Data.Entities;

public class XmlBuilder 
{
    public const string FORMAT_SET = "<Value><Table><Entry><Key>Name</Key><Value>{0}</Value></Entry><Entry><Key>CategoryId</Key><Value>{1}</Value></Entry><Entry><Key>Description</Key><Value>{2}</Value></Entry><Entry><Key>AssetSetId</Key><Value>{3}</Value></Entry><Entry><Key>CreatorName</Key><Value>{4}</Value></Entry><Entry><Key>ImageAssetId</Key><Value>{5}</Value></Entry><Entry><Key>SetType</Key><Value>{6}</Value></Entry></Table></Value>";
    public const string FORMAT_ASSET = "<Value><Table><Entry><Key>Name</Key><Value>{0}</Value></Entry><Entry><Key>AssetId</Key><Value>{1}</Value></Entry><Entry><Key>AssetSetId</Key><Value>{2}</Value></Entry><Entry><Key>AssetVersionId</Key><Value>{3}</Value></Entry><Entry><Key>CreatorName</Key><Value>{4}</Value></Entry><Entry><Key>IsTrusted</Key><Value>True</Value></Entry></Table></Value>";

    private string xml = "<List>";

    public void AppendPrivateSets(User user)
    {
        int baseId = user.Id * -8;

        xml += String.Format(FORMAT_SET, "My Models", baseId, "A set of my models.", baseId, user.UserName, 21267705, "private");

        baseId += 1;

        xml += String.Format(FORMAT_SET, "My Decals", baseId, "A set of my decals.", baseId, user.UserName, 21002577, "private");
    }

    public void AppendSet(AssetSet set, string type)
    {
        xml += String.Format(FORMAT_SET, set.Name, set.Id, set.Description, set.Id, set.CreatorName, set.ImageAssetId, type);
    }

    public void AppendAsset(Asset asset)
    {
        xml += String.Format(FORMAT_ASSET, asset.AssetName, asset.AssetId, asset.AssetSetId, asset.AssetVersionId, asset.CreatorName);
    }

    public string Get()
    {
        return xml + "</List>";
    }
}