using WalletTracker.Domain.Entities;
using WalletTracker.Domain.Enums;

namespace WalletTracker.Application.Interfaces;

public interface ICurrentUser
{
    public bool IsAuthenticated { get; }
    public Guid Id { get; }
    public string Email { get; }
    public UserRole UserRole { get; }
}
