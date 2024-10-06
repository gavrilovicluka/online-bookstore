using Bookstore.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Bookstore.Domain.Contracts;

public interface IUserRepository
{
    Task<IdentityResult> RegisterUser(User newUser, string password);
    Task<User?> GetUserByUsername(string username);
    Task<bool> CheckPassword(User user, string password);
    Task<IdentityResult> AddRole(string role);
    Task<IList<string>> GetUserRoles(User user);
    Task<IdentityResult> AddRoleToUser(User user, string role);
}