using Bookstore.Domain.Contracts;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Bookstore.Application.CQRS.Users.Commands.CreateRole;

public sealed class CreateRoleHandler : IRequestHandler<CreateRoleCommand, IdentityResult>
{
    private readonly IUserRepository _userRepository;

    public CreateRoleHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IdentityResult> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        return await _userRepository.AddRole(request.RoleName);
    }
}