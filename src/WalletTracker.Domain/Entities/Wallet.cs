using WalletTracker.Domain.Common;

namespace WalletTracker.Domain.Entities;

public class Wallet : BaseEntity
{
    public string Name { get; }
    public decimal Balance { get; private set; }

    // Nav property
    public Guid UserID { get; }

    public Wallet(string name, Guid userID)
    {
        Name = name;
        Balance = 0;
        UserID = userID;
    }

    private Wallet() { }

    public void Deposit(decimal amount)
    {
        Balance += amount;
        SetUpdatedAtUTC();
    }

    public void Withdraw(decimal amount)
    {
        Balance -= amount;
        SetUpdatedAtUTC();
    }
}
