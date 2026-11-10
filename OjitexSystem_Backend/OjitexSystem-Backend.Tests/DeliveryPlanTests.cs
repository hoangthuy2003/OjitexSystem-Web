using Microsoft.EntityFrameworkCore;
using OjitexSystem_Backend.Data.Production;
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
}
