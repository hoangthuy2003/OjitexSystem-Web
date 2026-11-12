namespace OjitexSystem_Backend.Repositories.Logistics;

public interface IPaperRollImportRepository
{
    Task<IReadOnlyList<PaperRollImportRecord>> SearchAsync(
        string? paperLabel,
        string? gradeName,
        DateOnly? dateReceived,
        CancellationToken cancellationToken = default);
}
