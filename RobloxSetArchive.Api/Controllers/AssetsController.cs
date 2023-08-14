// file:///home/pizzaboxer/Documents/Projects/RobloxSetArchive/dotnet-vue/RobloxSetArchive.Api/Controllers/AssetsController.cs {"mtime":1671327540325,"ctime":1671327467336,"size":1278,"etag":"39pmop2741a7","orphaned":false,"typeId":""}
using Microsoft.AspNetCore.Mvc;
using RobloxSetArchive.Api.Models;
using RobloxSetArchive.Api.Data;
using RobloxSetArchive.Api.Data.Entities;
using System.Text.Json;

namespace RobloxSetArchive.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AssetsController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;

    public AssetsController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet("{id}/Thumbnail")]
    [ResponseCache(VaryByHeader = "User-Agent", Duration = 30)]
    public async Task<IActionResult> GetThumbnail(int id)
    {
        // HttpClient httpClient = _httpClientFactory.CreateClient("Roblox");
        // HttpResponseMessage httpResponseMessage = await httpClient.GetAsync($"/asset/request-thumbnail-fix?assetId={id}&assetVersionId=0&imageFormat=Png&thumbnailFormatId=254&overrideModeration=false");

        // if (!httpResponseMessage.IsSuccessStatusCode)
        //     return StatusCode((int)httpResponseMessage.StatusCode);

        // string response = await httpResponseMessage.Content.ReadAsStringAsync();
        // ThumbnailApiModel? thumbnail = JsonSerializer.Deserialize<ThumbnailApiModel>(response);

        // if (thumbnail is null)
        //     return NotFound();

        // return Redirect(thumbnail.d.url);

        HttpClient httpClient = _httpClientFactory.CreateClient("Roblox");
        HttpResponseMessage httpResponseMessage = await httpClient.GetAsync($"/item-thumbnails?params=[{{assetId:{id}}}]");

        if (!httpResponseMessage.IsSuccessStatusCode)
            return StatusCode((int)httpResponseMessage.StatusCode);

        string response = await httpResponseMessage.Content.ReadAsStringAsync();
        List<ThumbnailApiModel>? thumbnails = JsonSerializer.Deserialize<List<ThumbnailApiModel>>(response);

        if (thumbnails is null || thumbnails.Count < 1)
            return NotFound();

        return Redirect(thumbnails[0].thumbnailUrl);
    }
}