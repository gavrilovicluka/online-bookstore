
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Bookstore.Application.CQRS.Users.Commands.RegisterUser;

public sealed record RegisterUserCommand(UserRegistrationDto UserRegistrationDto) : IRequest<IdentityResult>;