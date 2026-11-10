using OjitexSystem_Backend.Dtos.Logistics;
using OjitexSystem_Backend.Repositories.Auth;
using OjitexSystem_Backend.Repositories.Logistics;

namespace OjitexSystem_Backend.Services.Logistics;

public class DeliveryPlanService(IDeliveryPlanRepository repository, IUserRepository users) : IDeliveryPlanService
{
    public async Task<DeliveryPlanSearchResultDto> SearchAsync(
        DateOnly? deliveryDate, string? receiveNo,
        CancellationToken cancellationToken = default)
    {
        var items = await repository.SearchAsync(deliveryDate, receiveNo, cancellationToken);

        var userIds = items
            .SelectMany(item => new[] { item.UserFirstName, item.UserEditFirstName })
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

            if (!string.IsNullOrWhiteSpace(item.UserEditFirstName))
            {
                var id = item.UserEditFirstName.Trim();
                item.UserEditFirstName = firstNames.GetValueOrDefault(id);
            }
            else
            {
                item.UserEditFirstName = null;
            }
        }

        return new DeliveryPlanSearchResultDto { Items = items };
    }

    public async Task<DeliveryPlanMonthlyResultDto> GetMonthlyAsync(
        DateOnly? fromDate, DateOnly? toDate, string? productCode,
        CancellationToken cancellationToken = default)
    {
        productCode = string.IsNullOrWhiteSpace(productCode) ? null : productCode.Trim();

        var items = await repository.GetMonthlyAsync(fromDate, toDate, productCode, cancellationToken);

        var userIds = items
            .Where(item => !string.IsNullOrWhiteSpace(item.UserId))
            .Select(item => item.UserId!.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        var firstNames = userIds.Length > 0
            ? await users.GetFirstNamesAsync(userIds, cancellationToken)
            : new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);

        var matchedItems = new List<DeliveryPlanMonthlyItemDto>(items.Count);
        foreach (var item in items)
        {
            var userId = item.UserId?.Trim() ?? "";
            if (firstNames.TryGetValue(userId, out var firstName))
            {
                item.UserName = firstName;
                matchedItems.Add(item);
            }
        }

        return new DeliveryPlanMonthlyResultDto { Items = matchedItems };
    }
}
