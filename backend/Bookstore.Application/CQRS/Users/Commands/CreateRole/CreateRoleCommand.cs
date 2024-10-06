using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Bookstore.Application.CQRS.Users.Commands.CreateRole;

public sealed record CreateRoleCommand(string RoleName) : IRequest<IdentityResult>;