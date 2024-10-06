using Bookstore.Application.DTOs.Auth;
using MediatR;

namespace Bookstore.Application.CQRS.Users.Queries.LoginUser;

public sealed record LoginUserQuery(LoginDto LoginDto) : IRequest<LoginResponseDto>;