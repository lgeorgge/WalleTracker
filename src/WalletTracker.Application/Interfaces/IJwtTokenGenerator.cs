using WalletTracker.Application.Features.Auth;
using WalletTracker.Domain.Entities;

namespace WalletTracker.Application.Interfaces;

public interface IJwtTokenGenerator
{
    LoginResponse GenerateToken(User user);
}
