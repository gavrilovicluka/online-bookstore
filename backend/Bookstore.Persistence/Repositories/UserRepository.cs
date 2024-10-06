using Bookstore.Domain.Contracts;
using Bookstore.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Bookstore.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<User> _userManager;

    public UserRepository(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IdentityResult> RegisterUser(User newUser, string password)
    {
        return await _userManager.CreateAsync(newUser, password);
    }
}