namespace WalletTracker.Application.Common.Pagination;

public record PagedResponse<T>(
    int Page,
    int PageSize,
    int TotalCount,
    int TotalPages,
    IReadOnlyList<T> Data
);
