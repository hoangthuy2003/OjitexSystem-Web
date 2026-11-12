using OjitexSystem_Backend.Dtos.Logistics;

namespace OjitexSystem_Backend.Services.Logistics;

public interface IPaperRollImportService
{
    Task<PaperRollImportSearchResultDto> SearchAsync(
        string? paperLabel,
        string? gradeName,
        DateOnly? dateReceived,
        CancellationToken cancellationToken = default);
}
