using System.Globalization;
using Microsoft.EntityFrameworkCore;
using OjitexSystem_Backend.Data.Production;
using OjitexSystem_Backend.Dtos.Logistics;
using OjitexSystem_Backend.Repositories.Logistics;
using OjitexSystem_Backend.Services.Logistics;

namespace OjitexSystem_Backend.Tests;

public sealed class StockReportTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(104)]
    [InlineData(999)]
    [InlineData(1040100)]
    [InlineData(9999999)]
    public void NumericPrefixMatchesStringPrefixForAllProductCodeLengths(int prefix)
    {
        decimal?[] codes = [null, -1, 0, 1, 9, 10, 19, 100, 104, 999, 1000, 1040, 1049, 1050,
            9999, 10400, 10499, 10500, 99999, 104000, 104999, 105000, 999999, 1040000, 1040100, 1049999, 1050000, 9999999];
        var rows = codes.Select(code => new TMonthlyStock { MstProCd = code }).ToArray();
        var prefixText = prefix.ToString(CultureInfo.InvariantCulture);
        var expected = codes.Where(code => code.HasValue && code.Value.ToString(CultureInfo.InvariantCulture).StartsWith(prefixText)).ToArray();
        var actual = rows.AsQueryable().WhereProductCodeStartsWith(row => row.MstProCd, prefix)
            .Select(row => row.MstProCd).ToArray();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void NumericPrefixTranslatesOnSqlServerWithoutConvertingColumnToText()
    {
        using var context = new ProductionContext(new DbContextOptionsBuilder<ProductionContext>()
            .UseSqlServer("Server=unused;Database=unused;Integrated Security=true;TrustServerCertificate=true").Options);
        var sql = context.TCurrentStocks.WhereProductCodeStartsWith(row => row.CstProCd, 104).ToQueryString();
        Assert.Contains("WHERE", sql);
        Assert.Contains("1040000", sql);
        Assert.DoesNotContain("varchar", sql, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("LIKE", sql, StringComparison.OrdinalIgnoreCase);
        var nullableSql = context.TMonthlyStocks.WhereProductCodeStartsWith(row => row.MstProCd, 104).ToQueryString();
        Assert.Contains("WHERE", nullableSql);
    }

    [Theory]
    [InlineData(StockReportKind.Current, "2026-02-01", "2026-02-01")]
    [InlineData(StockReportKind.Last, "2026-01-01", "2026-01-31")]
    public async Task PeriodUsesBusinessTimezoneAcrossMonthBoundary(StockReportKind kind, string from, string to)
    {
        using var auth = new AuthTestFixture();
        var repository = new RecordingStockRepository();
        var service = new StockReportService(repository, auth.Users, auth.Clock);
        var report = await service.GetStockAsync(kind);
        Assert.Equal(DateOnly.Parse(from), report.WorkingDayFrom);
        Assert.Equal(DateOnly.Parse(to), report.WorkingDayTo);
    }

    [Fact]
    public async Task MonthlyHistorySelectsReportMonthAndEnrichesUserNames()
    {
        using var auth = new AuthTestFixture();
        auth.AddUser();
        var repository = new RecordingStockRepository();
        var service = new StockReportService(repository, auth.Users, auth.Clock);
        using var cancellation = new CancellationTokenSource();
        var history = await service.GetHistoryAsync(StockReportKind.Monthly, 1040100,
            new DateOnly(2024, 2, 20), cancellation.Token);
        Assert.Equal(new DateOnly(2024, 2, 1), repository.From);
        Assert.Equal(new DateOnly(2024, 2, 29), repository.To);
        Assert.Equal(cancellation.Token, repository.CancellationToken);
        Assert.Equal("Alice", history.Items[0].UserFirstName);
        Assert.Null(history.Items[1].UserFirstName);
    }

    [Fact]
    public async Task MonthlyHistoryWithoutReportDateDoesNotReadRepository()
    {
        using var auth = new AuthTestFixture();
        var repository = new RecordingStockRepository();
        var service = new StockReportService(repository, auth.Users, auth.Clock);
        await Assert.ThrowsAsync<ArgumentException>(() => service.GetHistoryAsync(StockReportKind.Monthly, 1040100));
        Assert.Null(repository.From);
    }

    [Fact]
    public async Task HistoryReadsOnlySnapshotForExactProductAndReportMonth()
    {
        using var context = new ProductionContext(new DbContextOptionsBuilder<ProductionContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
        context.TMonthlyStockHistories.AddRange(
            new TMonthlyStockHistory { MsthId = 1, MsthProCd = 1040100, MsthDateReport = new DateOnly(2024, 2, 20), MsthWorkDate = new DateOnly(2024, 1, 31) },
            new TMonthlyStockHistory { MsthId = 2, MsthProCd = 1040101, MsthDateReport = new DateOnly(2024, 2, 20) },
            new TMonthlyStockHistory { MsthId = 3, MsthProCd = 1040100, MsthDateReport = new DateOnly(2024, 3, 20) });
        await context.SaveChangesAsync();
        var result = await new StockReportRepository(context).GetHistoryAsync(StockReportKind.Monthly, 1040100,
            new DateOnly(2024, 2, 1), new DateOnly(2024, 2, 29));
        Assert.Equal(1, Assert.Single(result.Items).Id);
    }

    [Theory]
    [InlineData(StockReportKind.Current)]
    [InlineData(StockReportKind.Last)]
    public async Task CurrentAndLastHistoryKeepSnapshotEntriesOutsidePeriod(StockReportKind kind)
    {
        using var context = new ProductionContext(new DbContextOptionsBuilder<ProductionContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
        context.TCurrentStockHistories.Add(new TCurrentStockHistory { CsthId = 1, CsthProCd = 1040100, CsthWorkDate = new DateOnly(2020, 1, 1) });
        context.TLastStockHistories.Add(new TLastStockHistory { LsthId = 2, LsthProCd = 1040100, LsthWorkDate = new DateOnly(2020, 1, 1) });
        await context.SaveChangesAsync();
        var result = await new StockReportRepository(context).GetHistoryAsync(kind, 1040100,
            new DateOnly(2026, 1, 1), new DateOnly(2026, 1, 31));
        Assert.Single(result.Items);
        Assert.Equal(new DateOnly(2020, 1, 1), result.Items[0].WorkDate);
    }

    private sealed class RecordingStockRepository : IStockReportRepository
    {
        public DateOnly? From { get; private set; }
        public DateOnly? To { get; private set; }
        public CancellationToken CancellationToken { get; private set; }

        public Task<IReadOnlyList<StockReportItemDto>> GetStockAsync(StockReportKind kind, decimal? productCode, CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyList<StockReportItemDto>>([]);

        public Task<StockReportHistoryDto> GetHistoryAsync(StockReportKind kind, decimal productCode, DateOnly from, DateOnly to, CancellationToken cancellationToken = default)
        {
            From = from;
            To = to;
            CancellationToken = cancellationToken;
            return Task.FromResult(new StockReportHistoryDto
            {
                ProductCode = productCode, WorkingDayFrom = from, WorkingDayTo = to,
                Items = [new StockReportHistoryItemDto { UserId = " alice " }, new StockReportHistoryItemDto { UserId = "unknown" }]
            });
        }
    }
}
