using Microsoft.EntityFrameworkCore;
using OjitexSystem_Backend.Data.Production;
using OjitexSystem_Backend.Repositories.Factory;
using OjitexSystem_Backend.Services.Factory;

namespace OjitexSystem_Backend.Tests;

public sealed class ConvertingHistoryTests
{
    [Fact]
    public async Task UsesSelectedDateNormalizesFiltersAndMapsHistorySampleWithCancellation()
    {
        var repository = new RecordingRepository([new ConvertingOnlineRow
        {
            Date = "20261019", Shift = 2, MachineName = "FDC   ", LotNo = 1280, OrderNo = 75142,
            CustomerCode = 115, CustomerName = "NITORI ", ProductCode = 1153161, ProductName = "TOPPER ",
            PlanQty = 30, InputQty = 36, OutputQty = 34, SurplusQty = 0, LossQty = 0,
            StartTime = "00002800", FinishTime = "00004500", DrivingSeconds = 30,
            SetSeconds = 966, StopSeconds = 0, RestSeconds = 0
        }]);
        var service = new ConvertingOnlineService(repository,
            new FixedTimeProvider(DateTimeOffset.Parse("2026-10-20T02:00:00Z")));
        using var cancellation = new CancellationTokenSource();
        var date = new DateOnly(2026, 10, 19);
        var result = await service.GetHistoryAsync(date, " fdc ", " 115 ", cancellation.Token);
        Assert.Equal(date, repository.Date);
        Assert.Equal("FDC", repository.Process);
        Assert.Equal("115", repository.ProductCode);
        Assert.Equal(cancellation.Token, repository.Token);
        Assert.Equal(date, result.ReportDate);
        Assert.Equal(new DateTime(2026, 10, 20, 9, 0, 0), result.RefreshedAt);
        var item = Assert.Single(result.Items);
        Assert.Equal(date, item.Date);
        Assert.Equal("FDC", item.MachineName);
        Assert.Equal(1280m, item.LotNo);
        Assert.Equal(75142m, item.OrderNo);
        Assert.Equal("NITORI", item.CustomerName);
        Assert.Equal(1153161m, item.ProductCode);
        Assert.Equal("TOPPER", item.ProductName);
        Assert.Equal(0m, item.ShortestQty);
        Assert.Equal("00:28", item.StartTime);
        Assert.Equal("00:45", item.FinishTime);
        Assert.Equal(0.5m, item.DrivingMinutes);
        Assert.Equal(16.1m, item.SetMinutes);
    }

    [Fact]
    public async Task EmptyFiltersRequestAllHistoryWithoutDefaultingToTodayOrAFixedMachine()
    {
        var repository = new RecordingRepository([new ConvertingOnlineRow
        {
            Date = "bad", MachineName = "OTHER", PlanQty = 30, OutputQty = 20
        }]);
        var result = await new ConvertingOnlineService(repository, TimeProvider.System)
            .GetHistoryAsync(null, " ", " ");
        Assert.Null(repository.Date);
        Assert.Null(repository.Process);
        Assert.Null(repository.ProductCode);
        Assert.Null(result.ReportDate);
        var item = Assert.Single(result.Items);
        Assert.Null(item.Date);
        Assert.Equal("OTHER", item.MachineName);
        Assert.Equal(10m, item.ShortestQty);
    }

    [Theory]
    [InlineData(false, false, false, "30,20,50,1280,1280,40,1080")]
    [InlineData(true, false, false, "20,50,1280,1280,40,1080")]
    [InlineData(false, true, false, "30,1280,1280,40,1080")]
    [InlineData(false, false, true, "30,20,1280,1280,40")]
    [InlineData(true, true, false, "1280,1280,40,1080")]
    [InlineData(true, false, true, "20,1280,1280,40")]
    [InlineData(false, true, true, "30,1280,1280,40")]
    [InlineData(true, true, true, "1280,1280,40")]
    public async Task FiltersCombineWithAndKeepInnerJoinsDuplicatesAndSortByStartBeforeShift(
        bool byDate, bool byProcess, bool byProduct, string expectedLots)
    {
        using var context = await SeedAsync();
        var rows = await new ConvertingOnlineRepository(context).GetHistoryAsync(
            byDate ? new DateOnly(2026, 10, 19) : null, byProcess ? "FDC" : null,
            byProduct ? "1153" : null);
        Assert.Equal(expectedLots.Split(',').Select(decimal.Parse), rows.Select(row => row.LotNo!.Value));
        Assert.All(rows, row => Assert.Equal("NITORI", row.CustomerName));
        Assert.All(rows, row => Assert.NotNull(row.ProductName));
        Assert.Empty(context.ChangeTracker.Entries());
    }

    [Theory]
    [InlineData("1153161", 5)]
    [InlineData("115316", 5)]
    [InlineData("153161", 0)]
    [InlineData("0115", 0)]
    [InlineData("0", 1)]
    public async Task ProductSearchUsesLiteralPrefixIncludingZero(string productCode, int expected)
    {
        using var context = await SeedAsync();
        context.TInputProducts.Add(new TInputProduct { ProCd = 0, ProName = "Zero" });
        var zero = Source("20261019", "FDC", 0, 999, "00010000");
        context.TConvertingOnlineReports.Add(zero);
        await context.SaveChangesAsync();
        var rows = await new ConvertingOnlineRepository(context).GetHistoryAsync(null, null, productCode);
        Assert.Equal(expected, rows.Count);
    }

    [Fact]
    public async Task RepositoryHonorsCancelledRequests()
    {
        using var context = await SeedAsync();
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() =>
            new ConvertingOnlineRepository(context).GetHistoryAsync(null, null, null, cancellation.Token));
    }

    private static async Task<TestProductionContext> SeedAsync()
    {
        var context = new TestProductionContext(new DbContextOptionsBuilder<ProductionContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
        context.TInputCustomers.Add(new TInputCustomer { CusCd = 115, CusName = "NITORI" });
        context.TInputProducts.AddRange(
            new TInputProduct { ProCd = 1153161, ProName = "TOPPER" },
            new TInputProduct { ProCd = 1152351, ProName = "OTHER PRODUCT" },
            new TInputProduct { ProCd = 1302011, ProName = "BOX" });
        context.TConvertingOnlineReports.AddRange(
            Source("20261019", "FDC   ", 1153161, 1280, "00002800"),
            Source("20261019", "FDC   ", 1153161, 1280, "00002800"),
            Source("20261019", "FG", 1153161, 20, "00001000", 1),
            Source("20261018", "FDC", 1153161, 30, "00000500"),
            Source("20261019", "FDC", 1152351, 1080, "00004500"),
            Source("20261019", "FDCX", 1153161, 40, "00003000"),
            Source("20261019", "EVOL", 1302011, 50, "00002000"),
            Source("20261019", "FDC", 999, 90, "00000100"));
        var missingCustomer = Source("20261019", "FDC", 1153161, 91, "00000100");
        missingCustomer.CorNo8CustomerCode = 999;
        context.TConvertingOnlineReports.Add(missingCustomer);
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();
        return context;
    }

    private static TConvertingOnlineReport Source(string date, string machine, decimal productCode,
        decimal lot, string startTime, decimal shift = 2) => new()
    {
        CorNo1Date = date, CorNo43MachineName = machine, CorNo9ProductCode = productCode,
        CorNo6LotNo = lot, CorNo7OrderNo = 75142, CorNo8CustomerCode = 115, CorNo3Shift = shift,
        CorNo30StartTime = startTime
    };

    private sealed class RecordingRepository(IReadOnlyList<ConvertingOnlineRow> rows) : IConvertingOnlineRepository
    {
        public DateOnly? Date { get; private set; }
        public string? Process { get; private set; }
        public string? ProductCode { get; private set; }
        public CancellationToken Token { get; private set; }
        public Task<IReadOnlyList<ConvertingOnlineRow>> GetTodayAsync(
            DateOnly date, string machineName, CancellationToken cancellationToken = default) =>
            throw new NotSupportedException();
        public Task<IReadOnlyList<ConvertingOnlineRow>> GetHistoryAsync(
            DateOnly? date, string? process, string? productCode, CancellationToken cancellationToken = default)
        {
            Date = date;
            Process = process;
            ProductCode = productCode;
            Token = cancellationToken;
            return Task.FromResult(rows);
        }
    }

    private sealed class TestProductionContext(DbContextOptions<ProductionContext> options) : ProductionContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TConvertingOnlineReport>().Property<int>("TestRowId");
            modelBuilder.Entity<TConvertingOnlineReport>().HasKey("TestRowId");
        }
    }
}
