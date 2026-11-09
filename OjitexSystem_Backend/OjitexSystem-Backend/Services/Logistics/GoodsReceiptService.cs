using OjitexSystem_Backend.Dtos.Logistics;
using OjitexSystem_Backend.Repositories.Auth;
using OjitexSystem_Backend.Repositories.Logistics;

namespace OjitexSystem_Backend.Services.Logistics;

public class GoodsReceiptService(IGoodsReceiptRepository repository, IUserRepository users) : IGoodsReceiptService
{
    public async Task<GoodsReceiptSearchResultDto> SearchAsync(
        DateOnly? dateReceipt, string? receiveNo,
        CancellationToken cancellationToken = default)
    {
        var items = await repository.SearchAsync(dateReceipt, receiveNo, cancellationToken);

        // Collect all distinct user IDs (input + changed) for a single batch lookup.
        var userIds = items
            .SelectMany(item => new[] { item.UserFirstName, item.UserChangedFirstName })
            .Where(id => !string.IsNullOrWhiteSpace(id))
            .Select(id => id!.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        IReadOnlyDictionary<string, string?> firstNames = userIds.Length > 0
            ? await users.GetFirstNamesAsync(userIds, cancellationToken)
            : new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);

        foreach (var item in items)
        {
            if (!string.IsNullOrWhiteSpace(item.UserFirstName))
            {
                var id = item.UserFirstName.Trim();
                item.UserFirstName = firstNames.GetValueOrDefault(id);
            }
            else
            {
                item.UserFirstName = null;
            }

            if (!string.IsNullOrWhiteSpace(item.UserChangedFirstName))
            {
                var id = item.UserChangedFirstName.Trim();
                item.UserChangedFirstName = firstNames.GetValueOrDefault(id);
            }
            else
            {
                item.UserChangedFirstName = null;
            }
        }

        return new GoodsReceiptSearchResultDto { Items = items };
    }
}
