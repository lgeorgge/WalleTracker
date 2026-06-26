using WalletTracker.Application.Features.Wallets;
using WalletTracker.Domain.Entities;

namespace WalletTracker.Application.Specifications.Wallets;

public class WalletsSpec : BaseSpecification<Wallet>
{
    public WalletsSpec(WalletsQueryParameters walletsQueryParameters)
    {
        if (!string.IsNullOrWhiteSpace(walletsQueryParameters.Search))
        {
            string keyword = walletsQueryParameters.Search;
            AddCriteria(W => W.Name.Contains(keyword));
        }
        if (walletsQueryParameters.UserId.HasValue)
        {
            var userId = walletsQueryParameters.UserId.Value;
            AddCriteria(W => W.UserId == userId);
        }

        ApplyPagination(
            (walletsQueryParameters.Page - 1) * walletsQueryParameters.PageSize,
            walletsQueryParameters.PageSize
        );
    }
}
