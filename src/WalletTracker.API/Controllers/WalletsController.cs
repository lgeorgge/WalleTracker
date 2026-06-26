using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalletTracker.Application.Features.Wallets;
using WalletTracker.Application.Interfaces;
using WalletTracker.Domain.Common.Exceptions;
using WalletTracker.Domain.Entities;

namespace WalletTracker.API.Controllers;

[ApiController]
[Route("api/wallets")]
public class WalletsControllerController : ControllerBase
{
    private readonly IRepository<Wallet> _repo;
    private readonly IUnitOfWork _unitOfWork;

    public WalletsControllerController(IRepository<Wallet> repo, IUnitOfWork unitOfWork)
    {
        _repo = repo;
        _unitOfWork = unitOfWork;
    }

    [Authorize(Roles = "Admin,User")]
    [HttpPost]
    public async Task<IActionResult> CreateWalletAsync(
        [FromBody] CreateWalletRequest request,
        [FromServices] ICurrentUser currentUser,
        CancellationToken cancellationToken
    )
    {
        if (!currentUser.IsAuthenticated)
            throw new UnauthorizedException("User is not authenticated.");

        var name = request.Name;
        var userId = currentUser.Id;

        var wallet = new Wallet(name, userId);

        await _repo.AddAsync(wallet, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var response = new WalletResponse(
            wallet.Id,
            wallet.UserId,
            wallet.Name,
            wallet.Balance,
            wallet.CreatedAtUTC
        );

        return Ok(response);
    }
}
