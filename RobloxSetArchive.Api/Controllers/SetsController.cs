// file:///home/pizzaboxer/Documents/Projects/RobloxSetArchive/dotnet-vue/RobloxSetArchive.Api/Controllers/SetsController.cs {"mtime":1671326046090,"ctime":1670195457130,"size":4605,"etag":"39pmmlnen4oh","orphaned":false,"typeId":""}
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using RobloxSetArchive.Api.Models;
using RobloxSetArchive.Api.Data;
using RobloxSetArchive.Api.Data.Entities;

namespace RobloxSetArchive.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SetsController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMemoryCache _memoryCache;

    public SetsController(ApplicationDbContext dbContext, IMemoryCache memoryCache)
    {
        _dbContext = dbContext;
        _memoryCache = memoryCache;
    }

    [HttpGet("Search")]
    public IActionResult Search(string? keyword, int page = 1)
    {
        IQueryable<AssetSet> query = _dbContext.AssetSets;

        if (!String.IsNullOrEmpty(keyword))
            query = query.Where(x => x.Name.ToLower().Contains(keyword.ToLower()));

        query = query.OrderBy(x => x.Id);

        return Ok(new EnumerableResponseModel<AssetSet>(query, 24, page));
    }

    [HttpGet("{id}")]
    public ActionResult<AssetSet> Get(int id)
    {
        AssetSet? assetSet = _dbContext.AssetSets.Find(id);

        if (assetSet is null)
            return NotFound();

        return assetSet;
    }
    
    [HttpGet("{id}/Assets")]
    public ActionResult<EnumerableResponseModel<Asset>> GetAssets(int id, int page = 1)
    {
        string cacheKey = $"Set_Assets_{id}_{page}";

        if (!_memoryCache.TryGetValue(cacheKey, out EnumerableResponseModel<Asset>? data) || data is null)
        {
            AssetSet? assetSet = _dbContext.AssetSets.Find(id);

            if (assetSet is null)
                return NotFound();

            IQueryable<Asset> assets = _dbContext.Assets.Where(x => x.AssetSetId == id);

            data = new EnumerableResponseModel<Asset>(assets, 24, page);

            _memoryCache.Set(cacheKey, data);
        }

        return Ok(data);
    }

    [HttpGet("{id}/Subscribers")]
    public ActionResult<EnumerableResponseModel<User>> GetSubscribers(int id, int page = 1)
    {
        AssetSet? assetSet = _dbContext.AssetSets.Find(id);

        if (assetSet is null)
            return NotFound();

        List<int> subscriberIds = _dbContext.Subscribers.Where(x => x.AssetSetId == id).Select(x => x.UserId).ToList();
        IQueryable<User> users = _dbContext.Users.Where(x => subscriberIds.Contains(x.Id));

        return Ok(new EnumerableResponseModel<User>(users, 24, page));
    }

    [HttpGet("LuaWebService")]
    public ContentResult GetLuaWebService(int? sid = null, int? nsets = null, string? type = null, int? userid = null)
    {
        string xml = "";
        XmlBuilder xmlBuilder = new();

        if (type is not null)
        {
            if (type == "user" && userid is not null)
            {
                User? user = _dbContext.Users.Find(userid);

                if (user is null)
                {
                    xml = "<List></List>";
                }
                else
                {
                    xmlBuilder.AppendPrivateSets(user);

                    List<int> setIds = _dbContext.Subscribers.Where(x => x.UserId == user.Id).Select(x => x.AssetSetId).ToList();
                    IEnumerable<AssetSet> sets = _dbContext.AssetSets.Where(x => setIds.Contains(x.Id)).OrderByDescending(x => x.Id);

                    foreach (AssetSet set in sets)
                        xmlBuilder.AppendSet(set, "user");
                            
                    xml = xmlBuilder.Get();
                }
            }
            else if (type == "base")
            {
                IEnumerable<AssetSet> sets = _dbContext.AssetSets.Where(x => x.Id == 2 || x.Id == 3 || x.Id == 4).ToList();

                foreach (AssetSet set in sets)
                    xmlBuilder.AppendSet(set, "base");

                xml = xmlBuilder.Get();
            }
        }
        else if (sid is not null)
        {
            if (sid < 0)
            {
                // private set, return empty list
                xml = "<List></List>";
            }
            else
            {
                AssetSet? assetSet = _dbContext.AssetSets.Find(sid);

                if (assetSet is null)
                    return new ContentResult { StatusCode = 404 };

                IEnumerable<Asset> assets = _dbContext.Assets.Where(x => x.AssetSetId == sid).ToList();
                    
                foreach (Asset asset in assets)
                    xmlBuilder.AppendAsset(asset);

                xml = xmlBuilder.Get();
            }
        }

        if (String.IsNullOrEmpty(xml))
        {
            return new ContentResult
            {
                StatusCode = 400
            };
        }

        return new ContentResult
        {
            Content = xml,
            ContentType = "text/html; charset=UTF-8"
        };
    }
}