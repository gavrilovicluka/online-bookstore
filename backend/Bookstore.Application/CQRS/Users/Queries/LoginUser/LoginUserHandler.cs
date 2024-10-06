using Bookstore.Application.DTOs.Auth;
using Bookstore.Application.JwtFeatures;
using Bookstore.Domain.Contracts;
using Bookstore.Domain.Exceptions;
using MediatR;

namespace Bookstore.Application.CQRS.Users.Queries.LoginUser;

public sealed class LoginUserHandler : IRequestHandler<LoginUserQuery, LoginResponseDto>
{
    private readonly IUserRepository _userRepository;
    private readonly JwtHandler _jwtHandler;

    public LoginUserHandler(IUserRepository userRepository, JwtHandler jwtHandler)
    {
        _userRepository = userRepository;
        _jwtHandler = jwtHandler;
    }
    
    public async Task<LoginResponseDto> Handle(LoginUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByUsername(request.LoginDto.Username!);
        
        if(user == null || !await _userRepository.CheckPassword(user, request.LoginDto.Password!))
        {
            var resp = new LoginResponseDto { ErrorMessage = "Invalid Authentication" };
            throw new UnauthorizedAccessException(resp.ErrorMessage);
        }

        var roles = await _userRepository.GetUserRoles(user);
        var token = _jwtHandler.CreateToken(user, roles);

        return new LoginResponseDto { IsAuthSuccessfull = true, Token = token };
    }
}
