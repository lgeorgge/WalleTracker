using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalletTracker.Application.Features.Auth;
using WalletTracker.Application.Interfaces;
using WalletTracker.Application.Specifications.Users;
using WalletTracker.Domain.Common.Exceptions;
using WalletTracker.Domain.Entities;

namespace WalletTracker.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IRepository<User> _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordService _passwordService;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthController(
        IRepository<User> userRepository,
        IUnitOfWork unitOfWork,
        IPasswordService passwordService,
        IJwtTokenGenerator jwtTokenGenerator
    )
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _passwordService = passwordService;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync(
        [FromBody] RegisterUserRequest registerUserRequest,
        CancellationToken cancellationToken
    )
    {
        // Check if email exists
        var userExist = await _userRepository.FindSingleOrDefaultAsync(
            new UserByEmailSpecification(registerUserRequest.Email),
            cancellationToken
        );

        if (userExist != null)
            return Conflict(new { Message = "A user with this email already exists." });

        try
        {
            var user = new User(
                registerUserRequest.FirstName,
                registerUserRequest.LastName,
                registerUserRequest.Email,
                "TEMP_PASSWORD",
                registerUserRequest.UserRole
            );

            var hashedPassword = _passwordService.Hash(user, registerUserRequest.Password);

            user.SetPasswordHash(hashedPassword);

            await _userRepository.AddAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var response = _jwtTokenGenerator.GenerateToken(user);

            return Ok(response);
        }
        catch (DomainException)
        {
            throw new DomainException("Error creating new user.");
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> LoginAsync(
        [FromBody] LoginRequest loginRequest,
        CancellationToken cancellationToken
    )
    {
        var user =
            await _userRepository.FindSingleOrDefaultAsync(
                new UserByEmailSpecification(loginRequest.Email),
                cancellationToken
            ) ?? throw new NotFoundException("This user is not registered.");

        if (!_passwordService.Verify(user, loginRequest.Password, user.PasswordHash))
        {
            throw new UnauthorizedException("Invalid email or password.");
        }

        var response = _jwtTokenGenerator.GenerateToken(user);

        return Ok(response);
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult GetMe([FromServices] ICurrentUser currentUser)
    {
        var response = new
        {
            currentUser.Id,
            currentUser.Email,
            currentUser.UserRole,
        };
        return Ok(response);
    }
}
