using FluentValidation;
using WalletTracker.Domain.Entities;

namespace WalletTracker.Application.Features.Auth;

public class LoginRequestValidator : AbstractValidator<User>
{
    public LoginRequestValidator()
    {
        RuleFor(U => U.Email).NotEmpty().MaximumLength(100);
        RuleFor(U => U.PasswordHash).NotEmpty();
    }
}
