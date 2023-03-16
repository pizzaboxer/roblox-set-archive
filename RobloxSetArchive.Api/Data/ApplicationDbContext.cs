// file:///home/pizzaboxer/Documents/Projects/RobloxSetArchive/dotnet-vue/RobloxSetArchive.Api/Data/ApplicationDbContext.cs {"mtime":1671391458116,"ctime":1670195095369,"size":840,"etag":"39pps6oaqr3","orphaned":false,"typeId":""}
using Microsoft.EntityFrameworkCore;
using RobloxSetArchive.Api.Data.Entities;

namespace RobloxSetArchive.Api.Data;

public class ApplicationDbContext : DbContext
{
    private readonly IConfiguration Configuration;

    public ApplicationDbContext(IConfiguration configuration)
    {
        Configuration = configuration;
    } 
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(Configuration.GetConnectionString("RobloxSetArchiveDatabase"));
        optionsBuilder.UseSnakeCaseNamingConvention();
    }

    public DbSet<Asset> Assets { get; set; } = null!;
    public DbSet<AssetSet> AssetSets { get; set; } = null!;
    public DbSet<Subscriber> Subscribers { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
}