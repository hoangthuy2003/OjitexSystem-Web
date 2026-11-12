using Microsoft.EntityFrameworkCore;
using OjitexSystem_Backend.Data.Production;

namespace OjitexSystem_Backend.Repositories.Logistics;

public sealed class PaperRollImportRepository(ProductionContext context) : IPaperRollImportRepository
{
    public async Task<IReadOnlyList<PaperRollImportRecord>> SearchAsync(
        string? paperLabel,
        string? gradeName,
        DateOnly? dateReceived,
        CancellationToken cancellationToken = default)
    {
        var query = context.TPaperRollImports.AsNoTracking().AsQueryable();

        if (!string.IsNullOrEmpty(paperLabel))
        {
            query = query.Where(item => item.PrimLabel.Trim().StartsWith(paperLabel));
        }

        if (!string.IsNullOrEmpty(gradeName))
        {
            query = query.Where(item =>
                item.PrimGradeName != null && item.PrimGradeName.Trim().StartsWith(gradeName));
        }

        if (dateReceived.HasValue)
        {
            var start = dateReceived.Value.ToDateTime(TimeOnly.MinValue);
            var end = start.AddDays(1);
            query = query.Where(item =>
                item.PrimDateReceived >= start && item.PrimDateReceived < end);
        }

        return await query
            .OrderByDescending(item => item.PrimDateReceived)
            .ThenBy(item => item.PrimLabel)
            .Select(item => new PaperRollImportRecord(
                item.PrimSn.Trim(),
                item.PrimLabel.Trim(),
                item.PrimGradeName == null ? null : item.PrimGradeName.Trim(),
                item.PrimGramage,
                item.PrimWidth,
                item.PrimImportKg,
                item.PrimPaperType == null ? null : item.PrimPaperType.Trim(),
                item.PrimSupplierName == null ? null : item.PrimSupplierName.Trim(),
                item.PrimContainerNo == null ? null : item.PrimContainerNo.Trim(),
                item.PrimDateReceived,
                item.PrimDateInput,
                item.PrimDateChanged,
                item.PrimUserId,
                item.PrimRemark))
            .ToListAsync(cancellationToken);
    }
}
