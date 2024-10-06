using Bookstore.Application.CQRS.Users.Commands.RegisterUser;
using Bookstore.Application.CQRS.Users.Queries.LoginUser;
using Bookstore.Application.DTOs.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Api.Controllers;

[AllowAnonymous]
public class AccountController : ControllerBase
{
    public AccountController(IMediator mediator) : base(mediator)
    {
    }

    /// <summary>
    /// Register user 
    /// </summary>
    /// <param name="userRegistrationDto"> User registration DTO </param>
    /// <returns> User registration response </returns>
    [HttpPost("Register")]
    public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationDto userRegistrationDto)
    {
        var result = await _mediator.Send(new RegisterUserCommand(userRegistrationDto));
        
        if (result.Succeeded)
        {
            return StatusCode(201);
        }

        return BadRequest(result.Errors);
    }

    /// <summary>
    /// Login user with username and password
    /// </summary>
    /// <param name="loginDto"> Login DTO with username and password </param>
    /// <returns> JWT token or error message </returns>
    [HttpPost("Login")]
    public async Task<IActionResult> LoginUser([FromBody] LoginDto loginDto)
    {
        return Ok(await _mediator.Send(new LoginUserQuery(loginDto)));
    }
}