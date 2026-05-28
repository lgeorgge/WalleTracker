using WalletTracker.Domain.Common;
using WalletTracker.Domain.Common.Exceptions;

namespace WalletTracker.Domain.Entities;

public class User : BaseEntity
{
    public string FirstName { get; }
    public string LastName { get; }
    public string Email { get; }
    public string PasswordHash { get; }

    private User() { }

    public User(string firstName, string lastName, string email, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new DomainException("First name cannot be empty.");

        if (string.IsNullOrWhiteSpace(lastName))
            throw new DomainException("Last name cannot be empty.");

        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("Email cannot be empty.");

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new DomainException("Password hash cannot be empty.");

        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        Email = email.Trim().ToLower();
        PasswordHash = passwordHash;
    }
}
