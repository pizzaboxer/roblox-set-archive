// file:///home/pizzaboxer/Documents/Projects/RobloxSetArchive/dotnet-vue/RobloxSetArchive.Api/Controllers/UsersController.cs {"mtime":1671327149371,"ctime":1671139646636,"size":2617,"etag":"39pmo91as2md","orphaned":false,"typeId":""}
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using RobloxSetArchive.Api.Models;
using RobloxSetArchive.Api.Data;
using RobloxSetArchive.Api.Data.Entities;
using System.Text.Json;

namespace RobloxSetArchive.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IMemoryCache _memoryCache;

    public UsersController(ApplicationDbContext dbContext, IHttpClientFactory httpClientFactory, IMemoryCache memoryCache)
    {
        _dbContext = dbContext;
        _httpClientFactory = httpClientFactory;
        _memoryCache = memoryCache;
    }

    [HttpGet("Search")]
    public IActionResult Search(string? keyword, int page = 1)
    {
        IQueryable<User> query = _dbContext.Users;

        if (!String.IsNullOrEmpty(keyword))
            query = query.Where(x => x.UserName.ToLower().Contains(keyword.ToLower()));

        query = query.OrderBy(x => x.Id);

        return Ok(new EnumerableResponseModel<User>(query, 24, page));
    }

    // [HttpGet("{id}")]
    // public ActionResult<User> Get(int id)
    // {
    //     User? user = _dbContext.Users.Find(id);

    //     if (user is null)
    //         return NotFound();

    //     return user;
    // }

    [HttpGet("{id}")]
    public ActionResult<UserResponseModel> Get(int id)
    {
        User? user = _dbContext.Users.Find(id);

        if (user is null)
            return NotFound();

        UserResponseModel response = new() { Id = user.Id, UserName = user.UserName };

        response.Owned = _dbContext.AssetSets.Where(x => x.CreatorId == id).OrderBy(x => x.Id);
        
        List<int> ownedSetIds = _dbContext.Subscribers.Where(x => x.UserId == id).Select(x => x.AssetSetId).ToList();
        response.Subscribed = _dbContext.AssetSets.Where(x => ownedSetIds.Contains(x.Id) && x.CreatorId != id);

        return response;
    }

    [HttpGet("{id}/Thumbnail")]
    public async Task<IActionResult> GetThumbnail(long id)
    {
        string cacheKey = $"User_Thumbnail_{id}";

        if (!_memoryCache.TryGetValue(cacheKey, out string? thumbnailUrl) || thumbnailUrl is null)
        {
            HttpClient httpClient = _httpClientFactory.CreateClient("Roblox");
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync($"/avatar-thumbnails?params=[{{userId:{id},imageSize:\"large\"}}]");

            if (!httpResponseMessage.IsSuccessStatusCode)
                return StatusCode((int)httpResponseMessage.StatusCode);

            string response = await httpResponseMessage.Content.ReadAsStringAsync();
            List<ThumbnailApiModel>? thumbnails = JsonSerializer.Deserialize<List<ThumbnailApiModel>>(response);

            if (thumbnails is null || thumbnails.Count < 1)
                return NotFound();

            thumbnailUrl = thumbnails[0].thumbnailUrl;

            var cacheOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(DateTime.Now.AddDays(1));

            _memoryCache.Set(cacheKey, thumbnailUrl, cacheOptions);
        }

        return Redirect(thumbnailUrl);
    }
}