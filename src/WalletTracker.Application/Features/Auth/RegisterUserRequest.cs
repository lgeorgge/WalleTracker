namespace WalletTracker.Application.Features.Auth;

public record RegisterUserRequest(string FirstName, string LastName, string Email, string Password);
