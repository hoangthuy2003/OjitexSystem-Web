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
}
