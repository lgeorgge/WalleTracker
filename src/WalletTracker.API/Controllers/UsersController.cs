using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WalletTracker.Application.Common.Pagination;
using WalletTracker.Application.Features.Users;
using WalletTracker.Application.Interfaces;
using WalletTracker.Application.Specifications.Users;
using WalletTracker.Domain.Entities;

namespace WalletTracker.API.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController(IRepository<User> userRepository, IUnitOfWork unitOfWork)
    : ControllerBase
{
    private readonly IRepository<User> _userRepository = userRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<PagedResponse<UserResponse>>> List(
        [FromQuery] UserQueryParameters parameters,
        CancellationToken cancellationToken
    )
    {
        var usersSpec = new UsersSpecification(parameters);
        var users = await _userRepository.ListAsync(usersSpec, cancellationToken);
        var count = await _userRepository.CountAsync(usersSpec, cancellationToken);

        var totalPages = (int)Math.Ceiling((double)count / parameters.PageSize);

        var usersResponse = users
            .Select(U => new UserResponse(U.Id, $"{U.FirstName} {U.LastName}", U.Email))
            .ToList();

        return Ok(
            new PagedResponse<UserResponse>(
                parameters.Page,
                parameters.PageSize,
                count,
                totalPages,
                usersResponse
            )
        );
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(id, cancellationToken);
        if (user is null)
        {
            return NotFound();
        }
        return Ok(new UserResponse(user.Id, $"{user.FirstName} {user.LastName}", user.Email));
    }

    [HttpGet("email/{email}")]
    public async Task<IActionResult> GetByEmail(string email, CancellationToken cancellationToken)
    {
        var emailSpec = new UserByEmailSpecification(email);
        var user = await _userRepository.FindFirstOrDefaultAsync(emailSpec, cancellationToken);
        if (user is null)
        {
            return NotFound();
        }
        return Ok(user);
    }
}
