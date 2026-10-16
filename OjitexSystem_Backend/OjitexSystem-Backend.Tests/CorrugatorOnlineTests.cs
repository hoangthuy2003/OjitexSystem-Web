using Microsoft.EntityFrameworkCore;
using OjitexSystem_Backend.Data.Production;
using OjitexSystem_Backend.Repositories.Factory;
using OjitexSystem_Backend.Services.Factory;

namespace OjitexSystem_Backend.Tests;

public sealed class CorrugatorOnlineTests
{
    [Fact]
    public async Task UsesBusinessDateAndForwardsCancellation()
    {
        var repository = new RecordingRepository([]);
        var service = new CorrugatorOnlineService(repository,
            new FixedTimeProvider(DateTimeOffset.Parse("2026-10-15T18:00:00Z")));
        using var cancellation = new CancellationTokenSource();
        var result = await service.GetTodayAsync(cancellation.Token);
        Assert.Equal(new DateOnly(2026, 10, 16), result.ReportDate);
        Assert.Equal(result.ReportDate, repository.Date);
        Assert.Equal(cancellation.Token, repository.Token);
        Assert.Equal(new DateTime(2026, 10, 16, 1, 0, 0), result.RefreshedAt);
    }

    [Theory]
    [InlineData(315, 1250, 234, 100)]
    [InlineData(9087, 562, 1800, 170)]
    [InlineData(381, 1440, 270, 121)]
    [InlineData(0, 1250, 18, 0)]
    [InlineData(100, 0, 60, 0)]
    [InlineData(100, 1250, 0, 0)]
    [InlineData(100, 1250, 1, 0)]
    public async Task CalculatesAverageSpeedUsingRoundedDriveMinutes(int quality, int length, int seconds, int speed)
    {
        var result = await CreateService(new TCorrugatorOnlineReport
        {
            CoorNo34QualityItem = quality, CoorNo3CorruLength = length, CoorNo38DriveTime = seconds
        }).GetTodayAsync();
        Assert.Equal((decimal)speed, result.Items[0].AverageSpeed);
    }

    [Fact]
    public async Task MapsDatesPaperAndSqlRoundingWithoutLosingNulls()
    {
        var result = await CreateService(new TCorrugatorOnlineReport
        {
            CoorNo41PlanDate = "261016", CoorNo40CorrugatedDatetime = "2610160802",
            CoorNo32Dfl = " JC 170 ", CoorNo31Sf2m = "   115  ", CoorNo30Sf2l = "JC 170  ",
            CoorNo38DriveTime = 3, CoorNo39StopTime = 87,
            CoorNo42DateInput = new DateTime(2026, 10, 16, 8, 25, 0)
        }).GetTodayAsync();
        var row = result.Items[0];
        Assert.Equal(new DateOnly(2026, 10, 16), row.PlanDate);
        Assert.Equal(row.PlanDate, row.CorrugatedDate);
        Assert.Equal("08:02", row.EndTime);
        Assert.Equal("JC 170", row.Glue);
        Assert.Equal("", row.Bm);
        Assert.Equal("JC 170", row.Bl);
        Assert.Null(row.Am);
        Assert.Null(row.OrderNo);
        Assert.Equal(0.1m, row.DriveMinutes);
        Assert.Equal(1.5m, row.StopMinutes);
        Assert.Equal(new DateTime(2026, 10, 16, 8, 25, 0), row.DateInput);
    }

    [Fact]
    public async Task InvalidDatesAndMissingNumbersRemainEmptyAndSpeedIsZero()
    {
        var result = await CreateService(new TCorrugatorOnlineReport
        {
            CoorNo41PlanDate = "261332", CoorNo40CorrugatedDatetime = "bad"
        }).GetTodayAsync();
        var row = result.Items[0];
        Assert.Null(row.PlanDate);
        Assert.Null(row.CorrugatedDate);
        Assert.Null(row.EndTime);
        Assert.Null(row.DriveMinutes);
        Assert.Null(row.StopMinutes);
        Assert.Equal(0m, row.AverageSpeed);
    }

    [Fact]
    public async Task RepositoryFiltersByMachineDateRatherThanPlanOrInputDateAndSortsByTime()
    {
        using var context = new TestProductionContext(new DbContextOptionsBuilder<ProductionContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
        context.TCorrugatorOnlineReports.AddRange(
            new TCorrugatorOnlineReport { CoorNo1OrderNo = 30, CoorNo40CorrugatedDatetime = "2610160838", CoorNo41PlanDate = "261015" },
            new TCorrugatorOnlineReport { CoorNo1OrderNo = 10, CoorNo40CorrugatedDatetime = "2610160800" },
            new TCorrugatorOnlineReport { CoorNo1OrderNo = 20, CoorNo40CorrugatedDatetime = "2610152359", CoorNo42DateInput = new DateTime(2026, 10, 16) },
            new TCorrugatorOnlineReport { CoorNo1OrderNo = 40, CoorNo40CorrugatedDatetime = "2610170000" },
            new TCorrugatorOnlineReport { CoorNo1OrderNo = 50 });
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();
        var rows = await new CorrugatorOnlineRepository(context).GetTodayAsync(new DateOnly(2026, 10, 16));
        Assert.Equal(new decimal?[] { 10, 30 }, rows.Select(row => row.CoorNo1OrderNo));
        Assert.Empty(context.ChangeTracker.Entries());
    }

    private static CorrugatorOnlineService CreateService(TCorrugatorOnlineReport row) =>
        new(new RecordingRepository([row]), new FixedTimeProvider(DateTimeOffset.Parse("2026-10-16T02:00:00Z")));

    private sealed class RecordingRepository(IReadOnlyList<TCorrugatorOnlineReport> rows) : ICorrugatorOnlineRepository
    {
        public DateOnly Date { get; private set; }
        public CancellationToken Token { get; private set; }
        public Task<IReadOnlyList<TCorrugatorOnlineReport>> GetTodayAsync(DateOnly date, CancellationToken cancellationToken = default)
        {
            Date = date;
            Token = cancellationToken;
            return Task.FromResult(rows);
        }
    }

    // Only the test model has a key so InMemory can seed this scaffolded keyless table.
    private sealed class TestProductionContext(DbContextOptions<ProductionContext> options) : ProductionContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TCorrugatorOnlineReport>().HasKey(row => row.CoorNo1OrderNo);
        }
    }
}
