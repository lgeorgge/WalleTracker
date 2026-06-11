namespace WalletTracker.Application.Features.Auth;

public sealed record LoginResponse(string AccessToken, DateTime ValidUntilUTC);
