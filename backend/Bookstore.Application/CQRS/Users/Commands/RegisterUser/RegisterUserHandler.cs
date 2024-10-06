using Bookstore.Domain.Contracts;
using Bookstore.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Bookstore.Application.CQRS.Users.Commands.RegisterUser;

public sealed class RegisterUserHandler : IRequestHandler<RegisterUserCommand, IdentityResult>
{
    private readonly IUserRepository _userRepository;

    public RegisterUserHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IdentityResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        User newUser = new User
        {
            FirstName = request.UserRegistrationDto.FirstName,
            LastName = request.UserRegistrationDto.LastName, 
            UserName = request.UserRegistrationDto.Username,
            Email = request.UserRegistrationDto.Email
        };

        var result = await _userRepository.RegisterUser(newUser, request.UserRegistrationDto.Password!);

        var roleResult = await _userRepository.AddRoleToUser(newUser, "Customer");
        
        if(result.Succeeded)
        {
            if(!roleResult.Succeeded)
            {
                return roleResult;
            }
        }

        return result;
    }
}