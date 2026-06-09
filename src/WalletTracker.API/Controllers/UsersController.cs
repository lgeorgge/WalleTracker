using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WalletTracker.Application.Common.Pagination;
using WalletTracker.Application.Features.Users;
using WalletTracker.Application.Interfaces;
using WalletTracker.Application.Specifications.Users;
using WalletTracker.Domain.Entities;

namespace WalletTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IRepository<User> userRepository, IUnitOfWork unitOfWork)
    : ControllerBase
{
    private readonly IRepository<User> _userRepository = userRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    [HttpGet]
    public async Task<IActionResult> List(
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

    // [HttpPost]
    // public async Task<IActionResult> Create(
    //     CreateUserRequest request,
    //     CancellationToken cancellationToken
    // )
    // {
    //     var checkEmail = await _userRepository.FindSingleOrDefaultAsync(
    //         new UserByEmailSpecification(request.Email),
    //         cancellationToken
    //     );
    //     if (checkEmail != null)
    //         return Conflict(new { Message = "A user with this email already exists." });

    //     var user = new User(request.FirstName, request.LastName, request.Email, "hashed");

    //     await _userRepository.AddAsync(user, cancellationToken);
    //     await _unitOfWork.SaveChangesAsync(cancellationToken);

    //     return CreatedAtAction(nameof(GetById), new { id = user.Id },
    //         new UserResponse(user.Id, $"{user.FirstName} {user.LastName}", user.Email)
    //     );
    // }
}
