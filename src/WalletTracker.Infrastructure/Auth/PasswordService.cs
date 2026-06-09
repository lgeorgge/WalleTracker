using Microsoft.AspNetCore.Identity;
using WalletTracker.Application.Interfaces;
using WalletTracker.Domain.Entities;

namespace WalletTracker.Infrastructure.Auth;

public class PasswordService(IPasswordHasher<User> passwordHasher) : IPasswordService
{
    private readonly IPasswordHasher<User> _passwordHasher = passwordHasher;

    public string Hash(User user, string password)
    {
        return _passwordHasher.HashPassword(user, password);
    }

    public bool Verify(User user, string password, string hashedPassword)
    {
        var verificationResult = _passwordHasher.VerifyHashedPassword(
            user,
            hashedPassword,
            password
        );

        return verificationResult != PasswordVerificationResult.Failed;
    }
}
