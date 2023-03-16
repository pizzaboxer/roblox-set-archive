// file:///home/pizzaboxer/Documents/Projects/RobloxSetArchive/dotnet-vue/RobloxSetArchive.Api/Models/EnumerableResponseModel.cs {"mtime":1670195594270,"ctime":1670195565510,"size":1147,"etag":"39npjcqlg160","orphaned":false,"typeId":""}
namespace RobloxSetArchive.Api.Models;

public class EnumerableResponseModel<T>
{
    public int FirstItem { get; set; }
    public int LastItem { get; set; }
    public int ItemCount { get; set; }
    public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();

    public EnumerableResponseModel(IQueryable<T> query, int maxItems, int page = 1)
    {
        if (page < 1)
            page = 1;

        int startingIndex = (page - 1) * maxItems;

        ItemCount = query.Count();

        if (startingIndex >= ItemCount)
            return;

        Items = query.Skip(startingIndex).Take(maxItems);
        FirstItem = startingIndex + 1;
        LastItem = startingIndex + Items.Count();
    }

    // public EnumerableResponseModel(IEnumerable<T> query, int maxItems, int page = 1)
    // {
    //     if (page < 1)
    //         page = 1;

    //     int startingIndex = (page - 1) * maxItems;

    //     ItemCount = query.Count();

    //     if (startingIndex >= ItemCount)
    //         return;

    //     Items = query.Skip(startingIndex).Take(maxItems);
    //     FirstItem = startingIndex + 1;
    //     LastItem = startingIndex + Items.Count();
    // }

    // public EnumerableResponseModel(IEnumerable<T> query, int maxItems, int page = 1)
    // {
    //     return EnumerableResponseModel(query, maxItems, page);
    // }
}