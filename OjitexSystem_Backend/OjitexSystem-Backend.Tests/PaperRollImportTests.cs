using Microsoft.EntityFrameworkCore;
using OjitexSystem_Backend.Data.Production;
using OjitexSystem_Backend.Repositories.Logistics;
using OjitexSystem_Backend.Services.Logistics;

namespace OjitexSystem_Backend.Tests;

public sealed class PaperRollImportTests
{
    private static ProductionContext NewContext() => new(new DbContextOptionsBuilder<ProductionContext>()
        .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);

    [Fact]
    public async Task SearchFiltersByLabelGradeAndReceivedDateAndResolvesUserName()
    {
        using var auth = new AuthTestFixture();
        var user = auth.AddUser("paper-input");
        user.UserFirstName = "Ms. Thu";
        auth.Context.SaveChanges();

        using var context = NewContext();
        context.TPaperRollImports.AddRange(
            new TPaperRollImport
            {
                PrimSn = "C16076973A",
                PrimLabel = "26KV0691",
                PrimGradeName = "FMVIK",
                PrimGramage = 175m,
                PrimWidth = 1900m,
                PrimImportKg = 2074m,
                PrimPaperType = "N",
                PrimSupplierName = "VINA KRAFT",
                PrimContainerNo = "HACU4220857",
                PrimDateReceived = new DateTime(2026, 11, 11, 14, 30, 0),
                PrimDateInput = new DateTime(2026, 11, 12, 8, 32, 0),
                PrimDateChanged = new DateTime(2026, 11, 12, 8, 33, 0),
                PrimUserId = "paper-input",
                PrimRemark = "Imported"
            },
            new TPaperRollImport
            {
                PrimSn = "C16076974A",
                PrimLabel = "26KV0692",
                PrimGradeName = "FMVIK",
                PrimDateReceived = new DateTime(2026, 11, 11, 9, 0, 0),
                PrimUserId = "paper-input"
            },
            new TPaperRollImport
            {
                PrimSn = "C16076975A",
                PrimLabel = "26KV0693",
                PrimGradeName = "FMVIK",
                PrimDateReceived = new DateTime(2026, 11, 12, 9, 0, 0),
                PrimUserId = "paper-input"
            },
            new TPaperRollImport
            {
                PrimSn = "C16076976A",
                PrimLabel = "26KV0694",
                PrimGradeName = "FMVIK",
                PrimDateReceived = new DateTime(2026, 11, 11, 9, 0, 0),
                PrimUserId = "missing-user"
            },
            new TPaperRollImport
            {
                PrimSn = "C16076977A",
                PrimLabel = "26KV0695",
                PrimGradeName = "TEST",
                PrimDateReceived = new DateTime(2026, 11, 11, 9, 0, 0),
                PrimUserId = "paper-input"
            },
            new TPaperRollImport
            {
                PrimSn = "C16076978A",
                PrimLabel = "OTHER001",
                PrimGradeName = "FMVIK",
                PrimDateReceived = new DateTime(2026, 11, 11, 9, 0, 0),
                PrimUserId = "paper-input"
            });
        await context.SaveChangesAsync();

        var service = new PaperRollImportService(new PaperRollImportRepository(context), auth.Users);

        var result = await service.SearchAsync(
            "26KV069", "FM", new DateOnly(2026, 11, 11));

        Assert.Equal(2, result.Items.Count);
        var item = Assert.Single(result.Items, item => item.SerialNo == "C16076973A");
        Assert.Equal("C16076973A", item.SerialNo);
        Assert.Equal("26KV0691", item.PaperLabel);
        Assert.Equal("FMVIK", item.GradeName);
        Assert.Equal(175m, item.Grammage);
        Assert.Equal(1900m, item.PaperWidth);
        Assert.Equal(2074m, item.ImportKg);
        Assert.Equal("N", item.PaperType);
        Assert.Equal("VINA KRAFT", item.SupplierName);
        Assert.Equal("HACU4220857", item.ContainerNo);
        Assert.Equal(new DateTime(2026, 11, 11, 14, 30, 0), item.DateReceived);
        Assert.Equal(new DateTime(2026, 11, 12, 8, 32, 0), item.DateInput);
        Assert.Equal(new DateTime(2026, 11, 12, 8, 33, 0), item.DateEdit);
        Assert.Equal("Ms. Thu", item.UserInput);
        Assert.Equal("Imported", item.Remark);
    }

    [Fact]
    public async Task SearchReturnsAllImportsWhenFiltersAreEmptyAndKeepsMissingUserExcluded()
    {
        using var auth = new AuthTestFixture();
        auth.AddUser("paper-input");
        using var context = NewContext();
        context.TPaperRollImports.AddRange(
            new TPaperRollImport
            {
                PrimSn = "C16076973A",
                PrimLabel = "26KV0691",
                PrimUserId = "paper-input"
            },
            new TPaperRollImport
            {
                PrimSn = "C16076974A",
                PrimLabel = "26KV0692",
                PrimUserId = "unknown"
            });
        await context.SaveChangesAsync();

        var service = new PaperRollImportService(new PaperRollImportRepository(context), auth.Users);

        var result = await service.SearchAsync(null, null, null);

        var item = Assert.Single(result.Items);
        Assert.Equal("26KV0691", item.PaperLabel);
    }
}
