using FluentValidation;
using WalletTracker.Application.Features.Auth;

namespace WalletTracker.Application.Features.Auth;

public class RegisterUserRequestValidator : AbstractValidator<RegisterUserRequest>
{
    public RegisterUserRequestValidator()
    {
        RuleFor(U => U.FirstName).MaximumLength(100).NotEmpty();
        RuleFor(U => U.LastName).MaximumLength(100).NotEmpty();
        RuleFor(U => U.Email).NotEmpty().EmailAddress().MaximumLength(255);
    }
}
