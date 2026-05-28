namespace WalletTracker.Domain.Common;

public abstract class BaseEntity
{
    public Guid Id { get; protected set; }
    public DateTime CreatedAtUTC { get; protected set; }
    public DateTime? UpdatedAtUTC { get; protected set; }

    public void SetUpdatedAtUTC() => UpdatedAtUTC = DateTime.UtcNow;

    // Protected Set : everyone can read, but only children can write
}
