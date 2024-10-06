using Bookstore.Domain.Contracts;
using Bookstore.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Bookstore.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<UserRole> _roleManager;

    public UserRepository(UserManager<User> userManager, RoleManager<UserRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<IdentityResult> RegisterUser(User newUser, string password)
    {
        return await _userManager.CreateAsync(newUser, password);
    }

    public async Task<User?> GetUserByUsername(string username)
    {
        return await _userManager.FindByNameAsync(username);        
    }
    
    public async Task<bool> CheckPassword(User user, string password)
    {
        return await _userManager.CheckPasswordAsync(user, password);        
    }

    public async Task<IdentityResult> AddRole(string role)
    {
        return await _roleManager.CreateAsync(new UserRole { Name = role });
    }
    
    public async Task<IdentityResult> AddRoleToUser(User user, string role)
    {
        return await _userManager.AddToRoleAsync(user, role);
    }

    public async Task<IList<string>> GetUserRoles(User user)
    {
        return await _userManager.GetRolesAsync(user);
    }
}