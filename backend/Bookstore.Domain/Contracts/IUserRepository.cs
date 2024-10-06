using Bookstore.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Bookstore.Domain.Contracts;

public interface IUserRepository
{
    Task<IdentityResult> RegisterUser(User newUser, string password);
}