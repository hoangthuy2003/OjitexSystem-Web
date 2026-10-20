using Microsoft.EntityFrameworkCore;
using OjitexSystem_Backend.Data.Production;
using OjitexSystem_Backend.Repositories.Factory;
using OjitexSystem_Backend.Services.Factory;

namespace OjitexSystem_Backend.Tests;

public sealed class ConvertingOnlineTests
{
    [Theory]
    [InlineData("FDC")]
    [InlineData("EVOL")]
    [InlineData("FG")]
    [InlineData("FAP")]
    [InlineData("FPS")]
    public async Task UsesBusinessDateAndForwardsMachineAndCancellation(string machineName)
    {
        var repository = new RecordingRepository([]);
        var service = new ConvertingOnlineService(repository,
            new FixedTimeProvider(DateTimeOffset.Parse("2026-10-15T18:00:00Z")));
        using var cancellation = new CancellationTokenSource();
        var result = await service.GetTodayAsync(machineName, cancellation.Token);
        Assert.Equal(new DateOnly(2026, 10, 16), result.ReportDate);
        Assert.Equal(result.ReportDate, repository.Date);
        Assert.Equal(machineName, repository.MachineName);
        Assert.Equal(cancellation.Token, repository.Token);
        Assert.Equal(new DateTime(2026, 10, 16, 1, 0, 0), result.RefreshedAt);
        Assert.Empty(result.Items);
    }

    [Fact]
    public async Task MapsAllColumnsAndRoundsMinutesLikeSql()
    {
        var result = await CreateService(new ConvertingOnlineRow
        {
            Shift = 1, LotNo = 5110, OrderNo = 70876, CustomerCode = 115,
            CustomerName = "NITORI  ", ProductCode = 1151773, ProductName = "TOPPER TOP  ",
            PlanQty = 27, InputQty = 33, OutputQty = 25, SurplusQty = 4, LossQty = 2,
            StartTime = "00083000", FinishTime = "00083700",
            DrivingSeconds = 24, SetSeconds = 405, StopSeconds = 3, RestSeconds = 0
        }).GetTodayAsync("FDC");
        var row = Assert.Single(result.Items);
        Assert.Equal(1m, row.Shift);
        Assert.Equal(5110m, row.LotNo);
        Assert.Equal(70876m, row.OrderNo);
        Assert.Equal(115m, row.CustomerCode);
        Assert.Equal("NITORI", row.CustomerName);
        Assert.Equal(1151773m, row.ProductCode);
        Assert.Equal("TOPPER TOP", row.ProductName);
        Assert.Equal(27m, row.PlanQty);
        Assert.Equal(33m, row.InputQty);
        Assert.Equal(25m, row.OutputQty);
        Assert.Equal(4m, row.SurplusQty);
        Assert.Equal(2m, row.LossQty);
        Assert.Equal(2m, row.ShortestQty);
        Assert.Equal("08:30", row.StartTime);
        Assert.Equal("08:37", row.FinishTime);
        Assert.Equal(0.4m, row.DrivingMinutes);
        Assert.Equal(6.8m, row.SetMinutes);
        Assert.Equal(0.1m, row.StopMinutes);
        Assert.Equal(0m, row.RestMinutes);
    }

    [Theory]
    [InlineData(27, 25, 2)]
    [InlineData(27, 27, 0)]
    [InlineData(27, 33, 0)]
    [InlineData(27, 0, 27)]
    [InlineData(3000, 1, 2999)]
    [InlineData(null, 20, 0)]
    [InlineData(27, null, 0)]
    [InlineData(null, null, 0)]
    public async Task ShortestQuantityMatchesSqlIifIncludingNulls(int? plan, int? output, int expected)
    {
        var result = await CreateService(new ConvertingOnlineRow { PlanQty = plan, OutputQty = output }).GetTodayAsync("FDC");
        Assert.Equal((decimal)expected, result.Items[0].ShortestQty);
    }

    [Theory]
    [InlineData("00000500", "00:05")]
    [InlineData("01235959", "23:59")]
    [InlineData("00000000", "00:00")]
    [InlineData("00240000", null)]
    [InlineData("00086000", null)]
    [InlineData("not-time", null)]
    [InlineData("0830", null)]
    [InlineData("", null)]
    [InlineData(null, null)]
    public async Task FormatsSourceTimeUsingSqlPositions(string? source, string? expected)
    {
        var result = await CreateService(new ConvertingOnlineRow { StartTime = source, FinishTime = source }).GetTodayAsync("FDC");
        Assert.Equal(expected, result.Items[0].StartTime);
        Assert.Equal(expected, result.Items[0].FinishTime);
    }

    [Fact]
    public async Task MissingValuesRemainEmptyAndAllDurationsUseDecimalSqlRounding()
    {
        var empty = (await CreateService(new ConvertingOnlineRow()).GetTodayAsync("FDC")).Items[0];
        Assert.Null(empty.Shift);
        Assert.Null(empty.CustomerName);
        Assert.Null(empty.ProductCode);
        Assert.Null(empty.PlanQty);
        Assert.Null(empty.LossQty);
        Assert.Null(empty.DrivingMinutes);
        Assert.Null(empty.SetMinutes);
        Assert.Null(empty.StopMinutes);
        Assert.Null(empty.RestMinutes);
        var rounded = (await CreateService(new ConvertingOnlineRow
        {
            DrivingSeconds = 2.9m, SetSeconds = 3, StopSeconds = 87, RestSeconds = -3
        }).GetTodayAsync("FDC")).Items[0];
        Assert.Equal(0m, rounded.DrivingMinutes);
        Assert.Equal(0.1m, rounded.SetMinutes);
        Assert.Equal(1.5m, rounded.StopMinutes);
        Assert.Equal(-0.1m, rounded.RestMinutes);
    }

    [Theory]
    [InlineData("FDC")]
    [InlineData("EVOL")]
    [InlineData("FG")]
    [InlineData("FAP")]
    [InlineData("FPS")]
    public async Task RepositoryFiltersExactDateAndMachineInnerJoinsAndKeepsRepeatedResults(
        string machineName)
    {
        using var context = new TestProductionContext(new DbContextOptionsBuilder<ProductionContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
        context.TInputCustomers.Add(new TInputCustomer { CusCd = 115, CusName = "NITORI" });
        context.TInputProducts.Add(new TInputProduct { ProCd = 1151773, ProName = "TOPPER TOP" });
        var later = Report(machine: machineName, lot: 5110, input: 33);
        var repeated = Report(machine: machineName, lot: 5110, input: 33);
        var lowerInput = Report(machine: machineName, lot: 5110, input: 32);
        var early = Report(machine: machineName, lot: 5090, input: 23);
        early.CorNo30StartTime = "00081100";
        var nextShift = Report(machine: machineName, lot: 5010, input: 30);
        nextShift.CorNo3Shift = 2;
        nextShift.CorNo30StartTime = "00080000";
        var missingCustomer = Report(machine: machineName);
        missingCustomer.CorNo8CustomerCode = 999;
        var missingProduct = Report(machine: machineName);
        missingProduct.CorNo9ProductCode = 999;
        var missingCode = Report(machine: machineName);
        missingCode.CorNo8CustomerCode = null;
        context.TConvertingOnlineReports.AddRange(later, repeated, lowerInput, early, nextShift,
            Report(machine: machineName, date: "20261015"), Report(machine: machineName, date: "20261017"),
            Report(machine: machineName, date: null), Report(machine: machineName, date: "202610160"),
            missingCustomer, missingProduct, missingCode);
        context.TConvertingOnlineReports.AddRange(new[] { "FDC", "EVOL", "FG", "FAP", "FPS", "OTHER" }
            .Where(machine => machine != machineName).Select(machine => Report(machine: machine)));
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();

        var rows = await new ConvertingOnlineRepository(context).GetTodayAsync(new DateOnly(2026, 10, 16), machineName);
        Assert.Equal(new decimal?[] { 5090, 5110, 5110, 5110, 5010 }, rows.Select(row => row.LotNo));
        Assert.Equal(new decimal?[] { 23, 32, 33, 33, 30 }, rows.Select(row => row.InputQty));
        Assert.Equal(2, rows.Count(row => row.LotNo == 5110 && row.InputQty == 33));
        var mapped = rows[2];
        Assert.Equal("NITORI", mapped.CustomerName);
        Assert.Equal("TOPPER TOP", mapped.ProductName);
        Assert.Equal(70876m, mapped.OrderNo);
        Assert.Equal(115m, mapped.CustomerCode);
        Assert.Equal(1151773m, mapped.ProductCode);
        Assert.Equal(27m, mapped.PlanQty);
        Assert.Equal(25m, mapped.OutputQty);
        Assert.Equal(4m, mapped.SurplusQty);
        Assert.Equal(2m, mapped.LossQty);
        Assert.Equal("00083000", mapped.StartTime);
        Assert.Equal("00083700", mapped.FinishTime);
        Assert.Equal(24m, mapped.DrivingSeconds);
        Assert.Equal(405m, mapped.SetSeconds);
        Assert.Equal(3m, mapped.StopSeconds);
        Assert.Equal(0m, mapped.RestSeconds);
        Assert.Empty(context.ChangeTracker.Entries());
    }

    [Fact]
    public async Task EvolSamplePreservesLargeShortfallAndLongRestTime()
    {
        var result = await CreateService(new ConvertingOnlineRow
        {
            Shift = 1, LotNo = 5350, OrderNo = 69836, CustomerCode = 506,
            CustomerName = "DENKO", ProductCode = 5060131, ProductName = "Nitto - Thùng carton",
            PlanQty = 3000, InputQty = 1, OutputQty = 1, SurplusQty = 0, LossQty = 0,
            StartTime = "00085800", FinishTime = "00130200",
            DrivingSeconds = 0, SetSeconds = 114, StopSeconds = 0, RestSeconds = 13308
        }).GetTodayAsync("EVOL");
        var row = Assert.Single(result.Items);
        Assert.Equal(2999m, row.ShortestQty);
        Assert.Equal(221.8m, row.RestMinutes);
        Assert.Equal(1.9m, row.SetMinutes);
        Assert.Equal("08:58", row.StartTime);
        Assert.Equal("13:02", row.FinishTime);
    }

    [Fact]
    public async Task FgSampleMapsQuantitiesAndMinutesFromTheProvidedReport()
    {
        var result = await CreateService(new ConvertingOnlineRow
        {
            Shift = 1, LotNo = 5090, OrderNo = 74269, CustomerCode = 533,
            CustomerName = "RRD", ProductCode = 5330230, ProductName = "A274P-BC 51",
            PlanQty = 453, InputQty = 453, OutputQty = 453, SurplusQty = 0, LossQty = 0,
            StartTime = "00071300", FinishTime = "00075100",
            DrivingSeconds = 1242, SetSeconds = 714, StopSeconds = 342, RestSeconds = 0
        }).GetTodayAsync("FG");
        var row = Assert.Single(result.Items);
        Assert.Equal(5090m, row.LotNo);
        Assert.Equal(74269m, row.OrderNo);
        Assert.Equal(533m, row.CustomerCode);
        Assert.Equal("RRD", row.CustomerName);
        Assert.Equal(5330230m, row.ProductCode);
        Assert.Equal("A274P-BC 51", row.ProductName);
        Assert.Equal(453m, row.PlanQty);
        Assert.Equal(453m, row.InputQty);
        Assert.Equal(453m, row.OutputQty);
        Assert.Equal(0m, row.ShortestQty);
        Assert.Equal("07:13", row.StartTime);
        Assert.Equal("07:51", row.FinishTime);
        Assert.Equal(20.7m, row.DrivingMinutes);
        Assert.Equal(11.9m, row.SetMinutes);
        Assert.Equal(5.7m, row.StopMinutes);
        Assert.Equal(0m, row.RestMinutes);
    }

    [Fact]
    public async Task FapSampleKeepsSeparateResultsAndCalculatesTheirShortfalls()
    {
        var first = new ConvertingOnlineRow
        {
            Shift = 2, LotNo = 5090, OrderNo = 74641, CustomerCode = 118,
            CustomerName = "SD VIETNAM", ProductCode = 1180130, ProductName = "Tấm carton Pad 220*480mm",
            PlanQty = 978, InputQty = 228, OutputQty = 912, SurplusQty = 0, LossQty = 0,
            StartTime = "00132500", FinishTime = "00134200"
        };
        var second = first with { InputQty = 241, OutputQty = 960 };
        var result = await CreateService(first, second).GetTodayAsync("FAP");
        Assert.Equal(2, result.Items.Count);
        Assert.Equal(new decimal?[] { 228, 241 }, result.Items.Select(row => row.InputQty));
        Assert.Equal(new decimal?[] { 912, 960 }, result.Items.Select(row => row.OutputQty));
        Assert.Equal(new decimal[] { 66, 18 }, result.Items.Select(row => row.ShortestQty));
        Assert.All(result.Items, row =>
        {
            Assert.Equal(2m, row.Shift);
            Assert.Equal(5090m, row.LotNo);
            Assert.Equal(74641m, row.OrderNo);
            Assert.Equal(118m, row.CustomerCode);
            Assert.Equal("SD VIETNAM", row.CustomerName);
            Assert.Equal(1180130m, row.ProductCode);
            Assert.Equal("Tấm carton Pad 220*480mm", row.ProductName);
            Assert.Equal(978m, row.PlanQty);
            Assert.Equal("13:25", row.StartTime);
            Assert.Equal("13:42", row.FinishTime);
        });
    }

    [Fact]
    public async Task FpsSamplePreservesShortfallsAndLongRestAndStopTimes()
    {
        var first = new ConvertingOnlineRow
        {
            Shift = 1, LotNo = 5040, OrderNo = 70163, CustomerCode = 701,
            CustomerName = "THIEN THANH", ProductCode = 7011680, ProductName = "S115*3/C175",
            PlanQty = 320, InputQty = 3, OutputQty = 3, SurplusQty = 0, LossQty = 0,
            StartTime = "00092200", FinishTime = "00123000",
            DrivingSeconds = 0, SetSeconds = 120, StopSeconds = 66, RestSeconds = 11070
        };
        var second = first with
        {
            InputQty = 326, OutputQty = 240, StartTime = "00123000", FinishTime = "00130400",
            DrivingSeconds = 144, SetSeconds = 342, StopSeconds = 1584, RestSeconds = 0
        };
        var result = await CreateService(first, second).GetTodayAsync("FPS");
        Assert.Equal(2, result.Items.Count);
        Assert.Equal(new decimal[] { 317, 80 }, result.Items.Select(row => row.ShortestQty));
        Assert.Equal(new decimal?[] { 3, 326 }, result.Items.Select(row => row.InputQty));
        Assert.Equal(new decimal?[] { 3, 240 }, result.Items.Select(row => row.OutputQty));
        Assert.Equal(new decimal?[] { 0, 2.4m }, result.Items.Select(row => row.DrivingMinutes));
        Assert.Equal(new decimal?[] { 2, 5.7m }, result.Items.Select(row => row.SetMinutes));
        Assert.Equal(new decimal?[] { 1.1m, 26.4m }, result.Items.Select(row => row.StopMinutes));
        Assert.Equal(new decimal?[] { 184.5m, 0 }, result.Items.Select(row => row.RestMinutes));
        Assert.Equal(new[] { "09:22", "12:30" }, result.Items.Select(row => row.StartTime));
        Assert.Equal(new[] { "12:30", "13:04" }, result.Items.Select(row => row.FinishTime));
        Assert.All(result.Items, row =>
        {
            Assert.Equal(1m, row.Shift);
            Assert.Equal(5040m, row.LotNo);
            Assert.Equal(70163m, row.OrderNo);
            Assert.Equal(701m, row.CustomerCode);
            Assert.Equal("THIEN THANH", row.CustomerName);
            Assert.Equal(7011680m, row.ProductCode);
            Assert.Equal(320m, row.PlanQty);
        });
    }

    [Theory]
    [InlineData("OTHER")]
    [InlineData("")]
    public async Task RejectsUnsupportedMachinesBeforeQuerying(string machineName)
    {
        var repository = new RecordingRepository([]);
        var service = new ConvertingOnlineService(repository, TimeProvider.System);
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.GetTodayAsync(machineName));
        Assert.Null(repository.MachineName);
    }

    private static TConvertingOnlineReport Report(string? date = "20261016", string machine = "FDC",
        decimal lot = 5110, decimal input = 33) => new()
    {
        CorNo1Date = date, CorNo43MachineName = machine, CorNo3Shift = 1,
        CorNo6LotNo = lot, CorNo7OrderNo = 70876, CorNo8CustomerCode = 115, CorNo9ProductCode = 1151773,
        CorNo12CaseQty = 27, CorNo13ReceiveQty = 999, CorNo14FeedQty = input,
        CorNo15FinishQty = 25, CorNo16SurplusQty = 4, CorNo17LossQtyOwn = 999, CorNo21LossQtyOwn = 2,
        CorNo30StartTime = "00083000", CorNo31FinishTime = "00083700",
        CorNo32DrivingTime = 24, CorNo33SetTime = 405, CorNo34StopTime = 3, CorNo35RestTime = 0
    };

    private static ConvertingOnlineService CreateService(params ConvertingOnlineRow[] rows) =>
        new(new RecordingRepository(rows), new FixedTimeProvider(DateTimeOffset.Parse("2026-10-16T02:00:00Z")));

    private sealed class RecordingRepository(IReadOnlyList<ConvertingOnlineRow> rows) : IConvertingOnlineRepository
    {
        public DateOnly Date { get; private set; }
        public string? MachineName { get; private set; }
        public CancellationToken Token { get; private set; }

        public Task<IReadOnlyList<ConvertingOnlineRow>> GetHistoryAsync(
            DateOnly? date, string? process, string? productCode, CancellationToken cancellationToken = default) =>
            throw new NotSupportedException();

        public Task<IReadOnlyList<ConvertingOnlineRow>> GetTodayAsync(DateOnly date, string machineName, CancellationToken cancellationToken = default)
        {
            Date = date;
            MachineName = machineName;
            Token = cancellationToken;
            return Task.FromResult(rows);
        }
    }

    // A generated shadow key lets InMemory seed duplicate rows without changing production mapping.
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
