using Microsoft.EntityFrameworkCore;
using OjitexSystem_Backend.Data.Production;
using OjitexSystem_Backend.Repositories.Logistics;
using OjitexSystem_Backend.Services.Logistics;

namespace OjitexSystem_Backend.Tests;

public sealed class DeliveryNoteTests
{
    private static ProductionContext NewContext() => new(new DbContextOptionsBuilder<ProductionContext>()
        .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);

    [Fact]
    public async Task SearchPrefersExactSlipLoadsItsDetailsAndResolvesUserName()
    {
        using var auth = new AuthTestFixture();
        auth.AddUser();
        using var context = NewContext();
        context.TLogiDeliveryNotes.AddRange(
            new TLogiDeliveryNote { DnKey = 1, DnDeliverySlip = " 275440 ", DnUserId = "alice" },
            new TLogiDeliveryNote { DnKey = 2, DnDeliverySlip = "27544 ", DnUserId = "alice", DnCusNa = "Customer" });
        context.TLogiDeliveryNoteDetails.AddRange(
            new TLogiDeliveryNoteDetail { DndKey = 1, DndDeliverySlip = "27544", DndLineNo = 2 },
            new TLogiDeliveryNoteDetail { DndKey = 2, DndDeliverySlip = "27544", DndLineNo = 1 },
            new TLogiDeliveryNoteDetail { DndKey = 3, DndDeliverySlip = "275440", DndLineNo = 1 });
        await context.SaveChangesAsync();
        var service = new DeliveryNoteService(new DeliveryNoteRepository(context), auth.Users);

        var note = await service.SearchAsync("2754");

        Assert.NotNull(note);
        Assert.Equal("27544", note.Header.DeliverySlip);
        Assert.Equal("Alice", note.Header.UserFirstName);
        Assert.Equal([1, 2], note.Details.Select(detail => detail.LineNo));
        Assert.All(note.Details, detail => Assert.Equal("27544", detail.DeliverySlip));
    }

    [Fact]
    public async Task SearchReturnsNullWhenNoSlipMatches()
    {
        using var auth = new AuthTestFixture();
        using var context = NewContext();
        var service = new DeliveryNoteService(new DeliveryNoteRepository(context), auth.Users);
        Assert.Null(await service.SearchAsync("999"));
    }

    [Fact]
    public async Task InformationFiltersLeftJoinsProductCalculatesTotalSqmAndOrdersByDeliveryDateDesc()
    {
        using var auth = new AuthTestFixture();
        auth.AddUser("alice");
        using var context = NewContext();
        context.TInputProducts.Add(new TInputProduct
        {
            ProCd = 4001000m,
            ProFlute = "-B",
            ProSheetSqu = 0.415m
        });
        context.TLogiDeliveryNotes.AddRange(
            new TLogiDeliveryNote
            {
                DnKey = 1,
                DnDeliverySlip = "71123   ",
                DnCusCd = 400m,
                DnCusNa = "CHI NHANH",
                DnDateDelivery = new DateOnly(2026, 11, 5),
                DnTruckNumber = "29G01876",
                DnCarType = "15",
                DnTransportCompany = "NTT",
                DnDateInput = new DateTime(2026, 11, 5, 8, 4, 0),
                DnUserId = "alice",
                DnState = "Inserted"
            },
            new TLogiDeliveryNote
            {
                DnKey = 2,
                DnDeliverySlip = "71124   ",
                DnCusCd = 400m,
                DnDateDelivery = new DateOnly(2026, 11, 6),
                DnUserId = "alice"
            },
            new TLogiDeliveryNote
            {
                DnKey = 3,
                DnDeliverySlip = "71125   ",
                DnCusCd = 400m,
                DnDateDelivery = new DateOnly(2026, 11, 7),
                DnUserId = "unknown"
            });
        context.TLogiDeliveryNoteDetails.AddRange(
            new TLogiDeliveryNoteDetail
            {
                DndKey = 1,
                DndDeliverySlip = "71123   ",
                DndLineNo = 1,
                DndReceiveNo = 0,
                DndProCd = 4001000m,
                DndProName = "30004804 - Thung CAN 250 ml",
                DndQty = 16800m,
                DndCsPo = "PO-1"
            },
            new TLogiDeliveryNoteDetail
            {
                DndKey = 2,
                DndDeliverySlip = "71124   ",
                DndLineNo = 1,
                DndProCd = 4009999m,
                DndQty = 100m
            },
            new TLogiDeliveryNoteDetail
            {
                DndKey = 3,
                DndDeliverySlip = "71125   ",
                DndLineNo = 1,
                DndProCd = 4001000m,
                DndQty = 50m
            });
        await context.SaveChangesAsync();

        var service = new DeliveryNoteService(new DeliveryNoteRepository(context), auth.Users);

        // Without date filter: both alice notes returned, ordered by DeliveryDate DESC (71124 then 71123), unknown user excluded
        var allForCus = await service.GetInformationAsync("711", "40", "400", null);
        Assert.Equal(2, allForCus.Items.Count);
        Assert.Equal("71124", allForCus.Items[0].DeliverySlip);
        Assert.Null(allForCus.Items[0].Flute);
        Assert.Null(allForCus.Items[0].TotalSqm);

        var matched = allForCus.Items[1];
        Assert.Equal("71123", matched.DeliverySlip);
        Assert.Equal("Alice", matched.UserFirstName);
        Assert.Equal("-B", matched.Flute);
        Assert.Equal(0.415m, matched.SheetSqm);
        Assert.Equal(6972.000m, matched.TotalSqm);

        // With exact date filter: only 71123 returned
        var byDate = await service.GetInformationAsync(null, null, null, new DateOnly(2026, 11, 5));
        var single = Assert.Single(byDate.Items);
        Assert.Equal("71123", single.DeliverySlip);
    }
}
