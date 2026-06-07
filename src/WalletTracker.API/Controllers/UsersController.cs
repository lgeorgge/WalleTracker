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

    [HttpPost]
    public async Task<IActionResult> Create()
    {
        var user = new User("George", "Sherif", "georgesheruf123@gmail.com", "hashed-password");

        await _userRepository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return Ok(user);
    }
}
