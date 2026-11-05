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
}
