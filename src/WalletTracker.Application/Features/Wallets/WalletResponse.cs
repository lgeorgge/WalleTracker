namespace WalletTracker.Application.Features.Wallets;

public sealed record WalletResponse(
    Guid Id,
    Guid UserId,
    string Name,
    decimal Balance,
    DateTime CreatedAtUTC
);
