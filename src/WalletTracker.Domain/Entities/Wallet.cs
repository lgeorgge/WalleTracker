using WalletTracker.Domain.Common;
using WalletTracker.Domain.Common.Exceptions;

namespace WalletTracker.Domain.Entities;

public class Wallet : BaseEntity
{
    public string Name { get; private set; }
    public decimal Balance { get; private set; }

    // Nav property
    public Guid UserId { get; }
    public User User { get; }

    public Wallet(string name, Guid userId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Wallet name cannot be empty.");

        if (userId == Guid.Empty)
            throw new DomainException("Invalid UserId.");

        Name = name;
        Balance = 0;
        UserId = userId;
        CreatedAtUTC = DateTime.UtcNow;
    }

    private Wallet() { }

    public void Deposit(decimal amount)
    {
        if (amount <= 0)
            throw new DomainException("Deposit amount must be greater than zero.");

        Balance += amount;
        SetUpdatedAtUTC();
    }

    public void Withdraw(decimal amount)
    {
        if (amount <= 0)
            throw new DomainException("Withdrawal amount must be greater than zero.");

        if (amount > Balance)
            throw new DomainException("Insufficent balance.");
        Balance -= amount;
        SetUpdatedAtUTC();
    }

    public void Rename(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new DomainException("Wallet name cannot be empty.");

        Name = newName.Trim();
        SetUpdatedAtUTC();
    }
}
