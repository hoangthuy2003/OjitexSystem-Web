using OjitexSystem_Backend.Data.Production;

namespace OjitexSystem_Backend.Dtos;

public sealed record CurrentStockPageDto(
    IReadOnlyList<TCurrentStock> Items,
    bool HasMore,
    int Page,
    int PageSize);
