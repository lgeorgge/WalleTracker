using WalletTracker.Domain.Common;
using WalletTracker.Domain.Common.Exceptions;
using WalletTracker.Domain.Enums;

namespace WalletTracker.Domain.Entities;

public class User : BaseEntity
{
    public string FirstName { get; }
    public string LastName { get; }
    public string Email { get; }
    public string PasswordHash { get; private set; }

    public UserRole UserRole { get; }

    private User() { }

    public User(
        string firstName,
        string lastName,
        string email,
        string passwordHash,
        UserRole userRole
    )
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new DomainException("First name cannot be empty.");

        if (string.IsNullOrWhiteSpace(lastName))
            throw new DomainException("Last name cannot be empty.");

        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("Email cannot be empty.");

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new DomainException("Password hash cannot be empty.");

        CreatedAtUTC = DateTime.UtcNow;
        UpdatedAtUTC = null;

        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        Email = email.Trim().ToLower();
        PasswordHash = passwordHash;
        UserRole = userRole;
    }

    public void SetPasswordHash(string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new DomainException("Password hash cannot be empty.");

        PasswordHash = passwordHash;
        UpdatedAtUTC = DateTime.UtcNow;
    }
}
