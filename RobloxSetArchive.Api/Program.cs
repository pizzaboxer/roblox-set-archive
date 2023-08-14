// file:///home/pizzaboxer/Documents/Projects/RobloxSetArchive/dotnet-vue/RobloxSetArchive.Api/Program.cs {"mtime":1671325756645,"ctime":1670195025296,"size":788,"etag":"39pmm9r9rpd","orphaned":false,"typeId":""}
using RobloxSetArchive.Api.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>();
builder.Services.AddHttpClient("Roblox", httpClient => 
{
    httpClient.BaseAddress = new Uri("https://www.roblox.com");
    httpClient.DefaultRequestHeaders.Add("User-Agent", "Roblox/WinInet");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
// }

app.Use(async (context, next) => 
{
    context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
    await next();
});

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();