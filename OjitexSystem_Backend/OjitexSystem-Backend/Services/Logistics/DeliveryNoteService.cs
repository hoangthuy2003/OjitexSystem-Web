using OjitexSystem_Backend.Dtos.Logistics;
using OjitexSystem_Backend.Repositories.Auth;
using OjitexSystem_Backend.Repositories.Logistics;

namespace OjitexSystem_Backend.Services.Logistics;

public class DeliveryNoteService(IDeliveryNoteRepository repository, IUserRepository users) : IDeliveryNoteService
{
    public async Task<DeliveryNoteDto?> SearchAsync(string deliverySlip, CancellationToken cancellationToken = default)
    {
        var header = await repository.FindHeaderAsync(deliverySlip.Trim(), cancellationToken);
        if (header is null) return null;

        // User names live in the EVOLIO database, so they are resolved through the user repository.
        if (!string.IsNullOrWhiteSpace(header.UserId))
        {
            var userId = header.UserId.Trim();
            var firstNames = await users.GetFirstNamesAsync([userId], cancellationToken);
            header.UserFirstName = firstNames.GetValueOrDefault(userId);
        }

        var details = await repository.GetDetailsAsync(header.DeliverySlip, cancellationToken);
        return new DeliveryNoteDto { Header = header, Details = details };
    }

    public async Task<DeliveryInformationDto> GetInformationAsync(
        string? deliverySlip, string? customerCode, string? productCode, DateOnly? deliveryDate,
        CancellationToken cancellationToken = default)
    {
        deliverySlip = string.IsNullOrWhiteSpace(deliverySlip) ? null : deliverySlip.Trim();
        customerCode = string.IsNullOrWhiteSpace(customerCode) ? null : customerCode.Trim();
        productCode = string.IsNullOrWhiteSpace(productCode) ? null : productCode.Trim();

        var items = await repository.GetInformationAsync(
            deliverySlip, customerCode, productCode, deliveryDate, cancellationToken);

        var userIds = items
            .Where(item => !string.IsNullOrWhiteSpace(item.UserId))
            .Select(item => item.UserId!.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        var firstNames = await users.GetFirstNamesAsync(userIds, cancellationToken);
        var matchedItems = new List<DeliveryInformationItemDto>(items.Count);

        foreach (var item in items)
        {
            var userId = item.UserId?.Trim() ?? "";
            if (firstNames.TryGetValue(userId, out var firstName))
            {
                item.UserFirstName = firstName;
                matchedItems.Add(item);
            }
        }

        return new DeliveryInformationDto { Items = matchedItems };
    }

    public async Task<DeliveryMonthlyReportDto> GetMonthlyAsync(
        string? deliverySlip, string? customerCode, string? productCode,
        DateOnly? fromDate, DateOnly? toDate,
        CancellationToken cancellationToken = default)
    {
        deliverySlip = string.IsNullOrWhiteSpace(deliverySlip) ? null : deliverySlip.Trim();
        customerCode = string.IsNullOrWhiteSpace(customerCode) ? null : customerCode.Trim();
        productCode = string.IsNullOrWhiteSpace(productCode) ? null : productCode.Trim();

        var items = await repository.GetMonthlyAsync(
            deliverySlip, customerCode, productCode, fromDate, toDate, cancellationToken);

        var userIds = items
            .Where(item => !string.IsNullOrWhiteSpace(item.UserId))
            .Select(item => item.UserId!.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        var firstNames = await users.GetFirstNamesAsync(userIds, cancellationToken);
        var matchedItems = new List<DeliveryMonthlyItemDto>(items.Count);

        foreach (var item in items)
        {
            var userId = item.UserId?.Trim() ?? "";
            if (firstNames.TryGetValue(userId, out var firstName))
            {
                item.UserFirstName = firstName;
                matchedItems.Add(item);
            }
        }

        return new DeliveryMonthlyReportDto { Items = matchedItems };
    }
}
