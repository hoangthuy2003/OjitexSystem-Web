using Microsoft.EntityFrameworkCore;
using OjitexSystem_Backend.Data.Production;
using OjitexSystem_Backend.Repositories.Logistics;
using OjitexSystem_Backend.Services.Logistics;

namespace OjitexSystem_Backend.Tests;

public sealed class GoodsReceiptTests
{
    private static ProductionContext NewContext() => new(new DbContextOptionsBuilder<ProductionContext>()
        .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);

    [Fact]
    public async Task SearchReturnsGoodsReceiptItemsWithCustomerNameAndResolvesUserNames()
    {
        using var auth = new AuthTestFixture();
        var u1 = auth.AddUser("user1");
        u1.UserFirstName = "UserOne";
        var u2 = auth.AddUser("user2");
        u2.UserFirstName = "UserTwo";
        auth.Context.SaveChanges();

        using var context = NewContext();
        context.TInputCustomers.Add(new TInputCustomer
        {
            CusCd = 518m,
            CusName = "DECATHLON OFFICE"
        });

        context.TLogiGoodsReceipts.AddRange(
            new TLogiGoodsReceipt
            {
                GrKey = 1,
                GrDateReceipt = new DateOnly(2026, 11, 9),
                GrReceiveNo = 588354m,
                GrCusCd = 518m,
                GrProCd = 5180061m,
                GrProName = "CARTON B25 (410X280X300)",
                GrQty = 79m,
                GrProductionSection = "FFG1",
                GrNote = "Thanh",
                GrDateInput = new DateTime(2026, 11, 9, 8, 30, 0),
                GrUserId = "user1",
                GrDateChanged = new DateTime(2026, 11, 9, 9, 0, 0),
                GrUserChanged = "user2"
            },
            new TLogiGoodsReceipt
            {
                GrKey = 2,
                GrDateReceipt = new DateOnly(2026, 11, 8),
                GrReceiveNo = 999999m,
                GrCusCd = 518m,
                GrProCd = 5180061m,
                GrDateInput = new DateTime(2026, 11, 8, 8, 0, 0),
                GrUserId = "user1"
            });
        await context.SaveChangesAsync();

        var service = new GoodsReceiptService(new GoodsReceiptRepository(context), auth.Users);

        // Search by dateReceipt
        var result = await service.SearchAsync(new DateOnly(2026, 11, 9), null);

        Assert.Single(result.Items);
        var item = result.Items[0];
        Assert.Equal(1, item.Key);
        Assert.Equal(588354m, item.ReceiveNo);
        Assert.Equal("DECATHLON OFFICE", item.CustomerName);
        Assert.Equal("UserOne", item.UserFirstName);
        Assert.Equal("UserTwo", item.UserChangedFirstName);
        Assert.Equal("FFG1", item.ProductionSection);
    }

    [Fact]
    public async Task SearchFiltersByReceiveNoPrefix()
    {
        using var auth = new AuthTestFixture();
        using var context = NewContext();

        context.TLogiGoodsReceipts.AddRange(
            new TLogiGoodsReceipt
            {
                GrKey = 1,
                GrReceiveNo = 588354m,
                GrDateInput = new DateTime(2026, 11, 9, 8, 30, 0)
            },
            new TLogiGoodsReceipt
            {
                GrKey = 2,
                GrReceiveNo = 123456m,
                GrDateInput = new DateTime(2026, 11, 9, 9, 0, 0)
            });
        await context.SaveChangesAsync();

        var service = new GoodsReceiptService(new GoodsReceiptRepository(context), auth.Users);

        var result = await service.SearchAsync(null, "588");
        Assert.Single(result.Items);
        Assert.Equal(588354m, result.Items[0].ReceiveNo);
    }

    [Fact]
    public async Task GetInformationFiltersByDateReceiveNoAndProductCode()
    {
        using var auth = new AuthTestFixture();
        var user = auth.AddUser("staff");
        user.UserFirstName = "Ms. Thanh";
        auth.Context.SaveChanges();

        using var context = NewContext();
        context.TInputCustomers.Add(new TInputCustomer
        {
            CusCd = 501m,
            CusName = "B Braun VN"
        });

        context.TLogiGoodsReceipts.AddRange(
            new TLogiGoodsReceipt
            {
                GrKey = 1,
                GrDateReceipt = new DateOnly(2026, 11, 10),
                GrReceiveNo = 581322m,
                GrCusCd = 501m,
                GrProCd = 5010037m,
                GrProName = "Carton Box",
                GrQty = 480m,
                GrProductionSection = "ST",
                GrNote = "09/11/2026",
                GrDateInput = new DateTime(2026, 11, 10, 7, 32, 0),
                GrUserId = "staff"
            },
            new TLogiGoodsReceipt
            {
                GrKey = 2,
                GrDateReceipt = new DateOnly(2026, 11, 10),
                GrReceiveNo = 581322m,
                GrCusCd = 501m,
                GrProCd = 5010099m,
                GrDateInput = new DateTime(2026, 11, 10, 7, 30, 0),
                GrUserId = "staff"
            });
        await context.SaveChangesAsync();

        var service = new GoodsReceiptService(new GoodsReceiptRepository(context), auth.Users);

        var report = await service.GetInformationAsync(
            new DateOnly(2026, 11, 10), "581322", "5010037");

        Assert.Single(report.Items);
        var item = report.Items[0];
        Assert.Equal(581322m, item.ReceiveNo);
        Assert.Equal(5010037m, item.ProductCode);
        Assert.Equal("B Braun VN", item.CustomerName);
        Assert.Equal("Ms. Thanh", item.UserFirstName);
        Assert.Equal(480m, item.Quantity);
        Assert.Equal("ST", item.ProductionSection);
    }
}
