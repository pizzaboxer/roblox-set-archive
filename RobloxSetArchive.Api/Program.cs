// file:///home/pizzaboxer/Documents/Projects/RobloxSetArchive/dotnet-vue/RobloxSetArchive.Api/Program.cs {"mtime":1671325756645,"ctime":1670195025296,"size":788,"etag":"39pmm9r9rpd","orphaned":false,"typeId":""}
using RobloxSetArchive.Api.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMemoryCache();
builder.Services.AddDbContext<ApplicationDbContext>();
builder.Services.AddHttpClient("Roblox", httpClient => 
{
    httpClient.BaseAddress = new Uri("https://www.roblox.com");
    httpClient.DefaultRequestHeaders.Add("User-Agent", "Roblox/WinInet");
});
builder.Services.AddHttpClient("RobloxThumbnails", httpClient => 
{
    httpClient.BaseAddress = new Uri("https://thumbnails.roblox.com");
    httpClient.DefaultRequestHeaders.Add("User-Agent", "Roblox/WinInet");
});

var app = builder.Build();

app.UseSwagger(options =>
{
    options.RouteTemplate = "api/swagger/{documentname}/swagger.json";
});

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/api/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = "api";
});

app.Use(async (context, next) => 
{
    context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
    await next();
});

app.MapControllers();
app.Run();