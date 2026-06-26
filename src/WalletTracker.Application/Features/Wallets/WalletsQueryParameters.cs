namespace WalletTracker.Application.Features.Wallets;

public class WalletsQueryParameters : BaseQueryParameters
{
    public string? Search { get; set; }
    public Guid? UserId { get; set; }
}
