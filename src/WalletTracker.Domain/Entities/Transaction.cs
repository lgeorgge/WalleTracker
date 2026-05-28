using WalletTracker.Domain.Common;
using WalletTracker.Domain.Enums;

namespace WalletTracker.Domain.Entities;

public class Transaction : BaseEntity
{
    public decimal Amount { get; }

    public string Description { get; }

    public TransactionType Type { get; }

    public Guid WalletId { get; }

    private Transaction() { }

    public Transaction(decimal amount, string description, TransactionType type, Guid walletId)
    {
        Amount = amount;
        Description = description;
        Type = type;
        WalletId = walletId;
    }
}
