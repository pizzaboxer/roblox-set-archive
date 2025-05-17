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
            HttpClient httpClient = _httpClientFactory.CreateClient("RobloxThumbnails");

            const int maxRetries = 5;
            const int delayMilliseconds = 500;
            List<ThumbnailApiModelContents>? thumbnails = null;

            for (int attempt = 0; attempt < maxRetries; attempt++)
            {
                HttpResponseMessage httpResponseMessage = await httpClient.GetAsync($"v1/assets?assetIds={id}&returnPolicy=PlaceHolder&size=420x420&format=webp");

                if (!httpResponseMessage.IsSuccessStatusCode)
                    return StatusCode((int)httpResponseMessage.StatusCode);

                string response = await httpResponseMessage.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var result = JsonSerializer.Deserialize<ThumbnailApiModel>(response, options);

                thumbnails = result?.Data;
                if (thumbnails == null || thumbnails.Count == 0)
                    return NotFound();

                var state = thumbnails[0].State;

                if (string.Equals(state, "Completed", StringComparison.OrdinalIgnoreCase))
                    break;

                if (string.Equals(state, "Blocked", StringComparison.OrdinalIgnoreCase))
                    return StatusCode(403, $"HTTP 403 Forbidden: Thumbnail status is 'Blocked'.");

                if (!string.Equals(state, "Pending", StringComparison.OrdinalIgnoreCase))
                    return StatusCode(500, $"HTTP 500 Internal Server Error: Unexpected thumbnail state: {state}");

                await Task.Delay(delayMilliseconds);
            }

            if (thumbnails == null || !string.Equals(thumbnails[0].State, "Completed", StringComparison.OrdinalIgnoreCase))
                return StatusCode(504, "HTTP 504 Gateway Timeout: Thumbnail still pending after retries");

            thumbnailUrl = thumbnails[0].ImageUrl;

            var cacheOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(DateTime.Now.AddDays(7));

            _memoryCache.Set(cacheKey, thumbnailUrl, cacheOptions);
        }

        return Redirect(thumbnailUrl);
    }
}