using OjitexSystem_Backend.Dtos.Logistics;
using OjitexSystem_Backend.Repositories.Auth;
using OjitexSystem_Backend.Repositories.Logistics;

namespace OjitexSystem_Backend.Services.Logistics;

public sealed class PaperRollImportService(
    IPaperRollImportRepository repository,
    IUserRepository users) : IPaperRollImportService
{
    public async Task<PaperRollImportSearchResultDto> SearchAsync(
        string? paperLabel,
        string? gradeName,
        DateOnly? dateReceived,
        CancellationToken cancellationToken = default)
    {
        var records = await repository.SearchAsync(paperLabel, gradeName, dateReceived, cancellationToken);
        var userIds = records
            .Where(record => !string.IsNullOrWhiteSpace(record.UserId))
            .Select(record => record.UserId!.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();
        var userNames = await users.GetFirstNamesAsync(userIds, cancellationToken);

        var items = records
            .Where(record =>
                !string.IsNullOrWhiteSpace(record.UserId)
                && userNames.ContainsKey(record.UserId.Trim()))
            .Select(record => new PaperRollImportItemDto
            {
                SerialNo = record.SerialNo,
                PaperLabel = record.PaperLabel,
                GradeName = record.GradeName,
                Grammage = record.Grammage,
                PaperWidth = record.PaperWidth,
                ImportKg = record.ImportKg,
                PaperType = record.PaperType,
                SupplierName = record.SupplierName,
                ContainerNo = record.ContainerNo,
                DateReceived = record.DateReceived,
                DateInput = record.DateInput,
                DateEdit = record.DateEdit,
                UserInput = userNames[record.UserId!.Trim()],
                Remark = record.Remark,
            })
            .ToArray();

        return new PaperRollImportSearchResultDto { Items = items };
    }
}
