using WalletTracker.Domain.Common;
using WalletTracker.Domain.Common.Exceptions;
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
        if (amount <= 0)
            throw new DomainException("Transaction amount must be greater than zero.");

        if (string.IsNullOrWhiteSpace(description))
            throw new DomainException("Transaction description is required.");

        if (walletId == Guid.Empty)
            throw new DomainException("Invalid wallet ID.");

        Amount = amount;
        Description = description;
        Type = type;
        WalletId = walletId;
    }
}
