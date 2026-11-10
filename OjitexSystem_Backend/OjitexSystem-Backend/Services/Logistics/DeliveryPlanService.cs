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
}
