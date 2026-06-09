using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using WalletTracker.Application.Features.Auth;
using WalletTracker.Application.Interfaces;
using WalletTracker.Application.Specifications.Users;
using WalletTracker.Domain.Entities;

namespace WalletTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
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
                "TEMP_PASSWORD"
            );

            var hashedPassword = _passwordService.Hash(user, registerUserRequest.Password);

            user.SetPasswordHash(hashedPassword);

            await _userRepository.AddAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var response = _jwtTokenGenerator.GenerateToken(user);

            return Ok(response);
        }
        catch (ValidationException)
        {
            return ValidationProblem("Error creating a new user.");
        }
    }
}
