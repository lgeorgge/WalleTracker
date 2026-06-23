using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using WalletTracker.Application.Interfaces;
using WalletTracker.Domain.Entities;
using WalletTracker.Domain.Enums;

namespace WalletTracker.Infrastructure.Auth;

public class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public bool IsAuthenticated =>
        _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;

    public Guid Id
    {
        get
        {
            var id = _httpContextAccessor?.HttpContext?.User.FindFirstValue(
                ClaimTypes.NameIdentifier
            );
            if (string.IsNullOrWhiteSpace(id))
                throw new UnauthorizedAccessException("User is not authenticated");
            return Guid.Parse(id);
        }
    }

    public string Email
    {
        get
        {
            var email = _httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrWhiteSpace(email))
                throw new UnauthorizedAccessException("User is not authenticated");
            return email;
        }
    }

    public UserRole UserRole
    {
        get
        {
            var role = _httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.Role);
            if (string.IsNullOrWhiteSpace(role))
                throw new UnauthorizedAccessException("User is not authenticated");
            return Enum.Parse<UserRole>(role);
        }
    }
}
