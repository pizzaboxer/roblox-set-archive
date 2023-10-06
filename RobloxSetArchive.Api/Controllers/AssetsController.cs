// file:///home/pizzaboxer/Documents/Projects/RobloxSetArchive/dotnet-vue/RobloxSetArchive.Api/Controllers/AssetsController.cs {"mtime":1671327540325,"ctime":1671327467336,"size":1278,"etag":"39pmop2741a7","orphaned":false,"typeId":""}
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using RobloxSetArchive.Api.Models;
using System.Text.Json;

namespace RobloxSetArchive.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AssetsController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IMemoryCache _memoryCache;

    public AssetsController(IHttpClientFactory httpClientFactory, IMemoryCache memoryCache)
    {
        _httpClientFactory = httpClientFactory;
        _memoryCache = memoryCache;
    }

    [HttpGet("{id}/Thumbnail")]
    [ResponseCache(VaryByHeader = "User-Agent", Duration = 30)]
    public async Task<IActionResult> GetThumbnail(int id)
    {
        string cacheKey = $"Asset_Thumbnail_{id}";

        if (!_memoryCache.TryGetValue(cacheKey, out string? thumbnailUrl) || thumbnailUrl is null)
        {
            HttpClient httpClient = _httpClientFactory.CreateClient("Roblox");
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync($"/item-thumbnails?params=[{{assetId:{id}}}]");

            if (!httpResponseMessage.IsSuccessStatusCode)
                return StatusCode((int)httpResponseMessage.StatusCode);

            string response = await httpResponseMessage.Content.ReadAsStringAsync();
            List<ThumbnailApiModel>? thumbnails = JsonSerializer.Deserialize<List<ThumbnailApiModel>>(response);

            if (thumbnails is null || thumbnails.Count < 1)
                return NotFound();

            thumbnailUrl = thumbnails[0].thumbnailUrl;

            var cacheOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(DateTime.Now.AddDays(7));

            _memoryCache.Set(cacheKey, thumbnailUrl, cacheOptions);
        }

        return Redirect(thumbnailUrl);
    }
}