using WalletTracker.Domain.Entities;

namespace WalletTracker.Application.Specifications.Wallets;

public class WalletByNameSpec : BaseSpecification<Wallet>
{
    public WalletByNameSpec(string name)
    {
        AddCriteria(W => W.Name.ToLower() == name.ToLower());
    }
}
