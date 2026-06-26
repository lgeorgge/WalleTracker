using FluentValidation;

namespace WalletTracker.Application.Features.Wallets;

public class CreateWalletRequestValidator : AbstractValidator<CreateWalletRequest>
{
    public CreateWalletRequestValidator()
    {
        RuleFor(W => W.Name)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("Wallet name must not exceed 100 characters.");
    }
}
