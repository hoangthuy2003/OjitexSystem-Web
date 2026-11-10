using Microsoft.EntityFrameworkCore;
using OjitexSystem_Backend.Controllers.Logistics;
using OjitexSystem_Backend.Data.Production;
using OjitexSystem_Backend.Dtos.Logistics;
using OjitexSystem_Backend.Repositories.Logistics;
using OjitexSystem_Backend.Services.Logistics;

namespace OjitexSystem_Backend.Tests;

public sealed class DeliveryPlanTests
{
    private static ProductionContext NewContext() => new(new DbContextOptionsBuilder<ProductionContext>()
        .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);

    [Fact]
    public async Task SearchReturnsDeliveryPlanItemsWithCurrentStockAndResolvesUserNames()
    {
        using var auth = new AuthTestFixture();
        var u1 = auth.AddUser("user1");
        u1.UserFirstName = "Mr. Input";
        var u2 = auth.AddUser("user2");
        u2.UserFirstName = "Mr. Edit";
        auth.Context.SaveChanges();

        using var context = NewContext();
        context.TCurrentStocks.Add(new TCurrentStock
        {
            CstProCd = 1001m,
            CstStock = 500m
        });

        context.TLogiDeliveryPlans.AddRange(
            new TLogiDeliveryPlan
            {
                DpId = 1,
                DpDateDelivery = new DateOnly(2026, 11, 10),
                DpCusNa = "Customer A",
                DpReceiveNo = 123456m,
                DpProCd = 1001m,
                DpProName = "Box A",
                DpQty = 100m,
                DpStock = 400m,
                DpAutualQty = 100m,
                DpPoNo = "PO-001",
                DpRemark = "Remark 1",
                DpSqm = 1.25m,
                DpTotalSqm = 125m,
                DpFlute = "AB",
                DpLastProcess = "Print",
                DpAcceptanceNote = "Accepted",
                DpDateInput = new DateTime(2026, 11, 9, 8, 0, 0),
                DpDateChanged = new DateTime(2026, 11, 9, 10, 0, 0),
                DpUserId = "user1",
                DpUserIdChanged = "user2"
            },
            new TLogiDeliveryPlan
            {
                DpId = 2,
                DpDateDelivery = new DateOnly(2026, 11, 11),
                DpReceiveNo = 999999m,
                DpProCd = 1002m,
                DpUserId = "user1"
            });
        await context.SaveChangesAsync();

        var service = new DeliveryPlanService(new DeliveryPlanRepository(context), auth.Users);

        var result = await service.SearchAsync(new DateOnly(2026, 11, 10), "123");

        Assert.Single(result.Items);
        var item = result.Items[0];
        Assert.Equal(1, item.AcceptanceNo);
        Assert.Equal(1001m, item.ProductCode);
        Assert.Equal("Customer A", item.CustomerName);
        Assert.Equal(500m, item.CurrentStock);
        Assert.Equal("Mr. Input", item.UserFirstName);
        Assert.Equal("Mr. Edit", item.UserEditFirstName);
    }

    [Fact]
    public async Task GetMonthlyReturnsDeliveryPlanMonthlyItemsAndFiltersByDatesAndProductCode()
    {
        using var auth = new AuthTestFixture();
        var u1 = auth.AddUser("user1");
        u1.UserFirstName = "Ms. Monthly";
        auth.Context.SaveChanges();

        using var context = NewContext();
        context.TCurrentStocks.Add(new TCurrentStock
        {
            CstProCd = 2001m,
            CstStock = 350m
        });

        context.TLogiDeliveryPlans.AddRange(
            new TLogiDeliveryPlan
            {
                DpId = 10,
                DpDateDelivery = new DateOnly(2026, 11, 5),
                DpCusNa = "Customer Monthly",
                DpReceiveNo = 654321m,
                DpProCd = 2001m,
                DpProName = "Box Monthly",
                DpQty = 200m,
                DpStock = 150m,
                DpAutualQty = 200m,
                DpPoNo = "PO-M1",
                DpRemark = "Monthly Remark",
                DpSqm = 2.0m,
                DpTotalSqm = 400m,
                DpFlute = "B",
                DpLastProcess = "Cut",
                DpDateInput = new DateTime(2026, 11, 1, 9, 0, 0),
                DpDateChanged = new DateTime(2026, 11, 2, 9, 0, 0),
                DpUserId = "user1"
            },
            new TLogiDeliveryPlan
            {
                DpId = 11,
                DpDateDelivery = new DateOnly(2026, 10, 25), // out of date range
                DpProCd = 2001m,
                DpUserId = "user1"
            },
            new TLogiDeliveryPlan
            {
                DpId = 12,
                DpDateDelivery = new DateOnly(2026, 11, 5),
                DpProCd = 3001m, // different product code
                DpUserId = "user1"
            },
            new TLogiDeliveryPlan
            {
                DpId = 13,
                DpDateDelivery = new DateOnly(2026, 11, 5),
                DpProCd = 2001m,
                DpUserId = "unknown_user" // inner join excludes missing user
            });
        await context.SaveChangesAsync();

        var service = new DeliveryPlanService(new DeliveryPlanRepository(context), auth.Users);

        var result = await service.GetMonthlyAsync(
            new DateOnly(2026, 11, 1),
            new DateOnly(2026, 11, 10),
            "2001");

        Assert.Single(result.Items);
        var item = result.Items[0];
        Assert.Equal(10, item.AcceptanceNo);
        Assert.Equal(2001m, item.ProductCode);
        Assert.Equal("Customer Monthly", item.CustomerName);
        Assert.Equal(350m, item.CurrentStock);
        Assert.Equal("Ms. Monthly", item.UserName);
    }

    private sealed class StubDeliveryPlanService : IDeliveryPlanService
    {
        public DateOnly? LastFromDate { get; private set; }
        public DateOnly? LastToDate { get; private set; }

        public Task<DeliveryPlanSearchResultDto> SearchAsync(DateOnly? deliveryDate, string? receiveNo, CancellationToken cancellationToken = default) =>
            Task.FromResult(new DeliveryPlanSearchResultDto());

        public Task<DeliveryPlanMonthlyResultDto> GetMonthlyAsync(DateOnly? fromDate, DateOnly? toDate, string? productCode, CancellationToken cancellationToken = default)
        {
            LastFromDate = fromDate;
            LastToDate = toDate;
            return Task.FromResult(new DeliveryPlanMonthlyResultDto());
        }
    }

    [Fact]
    public async Task ControllerParsesMonthDayYearDatesCorrectly()
    {
        var stub = new StubDeliveryPlanService();
        var controller = new DeliveryPlanController(stub);

        var result = await controller.GetMonthly("11/01/2026", "11/10/2026", "1340060", CancellationToken.None);

        Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(result.Result);
        Assert.Equal(new DateOnly(2026, 11, 1), stub.LastFromDate);
        Assert.Equal(new DateOnly(2026, 11, 10), stub.LastToDate);
    }
}
