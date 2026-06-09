using WalletTracker.Domain.Entities;

namespace WalletTracker.Application.Interfaces;

public interface IPasswordService
{
    string Hash(User user, string password);
    bool Verify(User user, string password, string hashedPassword);
}
