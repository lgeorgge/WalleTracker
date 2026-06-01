using Microsoft.AspNetCore.Mvc;
using WalletTracker.Application.Interfaces;
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
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _userRepository.GetAllAsync());
    }

    [HttpPost]
    public async Task<IActionResult> Create()
    {
        var user = new User("George", "Sherif", "george@test.com", "hashed-password");

        await _userRepository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return Ok(user);
    }
}
