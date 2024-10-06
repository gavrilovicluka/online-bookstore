using Bookstore.Application.CQRS.Users.Commands.RegisterUser;
using Bookstore.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Api.Controllers;

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
    [AllowAnonymous]
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
}