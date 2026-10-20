using System.Globalization;
using Microsoft.EntityFrameworkCore;
using OjitexSystem_Backend.Data.Production;

namespace OjitexSystem_Backend.Repositories.Factory;

public sealed class CorrugatorOnlineRepository(ProductionContext context) : ICorrugatorOnlineRepository
{
    public async Task<IReadOnlyList<TCorrugatorOnlineReport>> GetByDateAsync(
        DateOnly date, CancellationToken cancellationToken = default)
    {
        // The source stores yyMMdd and is interpreted with SQL Server's 2049 cutoff.
        if (date.Year is < 1950 or > 2049) return [];

        var prefix = date.ToString("yyMMdd", CultureInfo.InvariantCulture);
        return await context.TCorrugatorOnlineReports.AsNoTracking()
            .Where(row => row.CoorNo40CorrugatedDatetime != null
                && row.CoorNo40CorrugatedDatetime.StartsWith(prefix))
            .OrderBy(row => row.CoorNo40CorrugatedDatetime)
            .ThenBy(row => row.CoorNo1OrderNo)
            .ThenBy(row => row.CoorNo42DateInput)
            .ToListAsync(cancellationToken);
    }
}
