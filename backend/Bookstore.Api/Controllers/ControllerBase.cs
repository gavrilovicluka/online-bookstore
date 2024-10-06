using Bookstore.Domain.Exceptions;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ControllerBase : Controller
{
    protected readonly IMediator _mediator;

    public ControllerBase(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    protected void HandleValidationErrors(List<ValidationFailure> errors)
    {
        string errorMessage = "";
        
        foreach (var error in errors)
        {
            errorMessage += error.ErrorMessage + " ";
        }

        throw new BadRequestException(errorMessage);
    }
}