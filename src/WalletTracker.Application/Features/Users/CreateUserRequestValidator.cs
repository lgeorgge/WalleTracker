using FluentValidation;

namespace WalletTracker.Application.Features.Users;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(U => U.FirstName).MaximumLength(100).NotEmpty();
        RuleFor(U => U.LastName).MaximumLength(100).NotEmpty();
        RuleFor(U => U.Email).NotEmpty().EmailAddress().MaximumLength(255);
    }
}
