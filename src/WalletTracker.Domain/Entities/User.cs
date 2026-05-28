using WalletTracker.Domain.Common;

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
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PasswordHash = passwordHash;
    }
}
