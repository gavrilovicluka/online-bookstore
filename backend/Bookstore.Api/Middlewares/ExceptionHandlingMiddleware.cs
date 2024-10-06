using System.Net;
using Bookstore.Domain.Exceptions;

namespace Bookstore.Api.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            _logger.LogError("An unexpected error occurred.");
            await HandleExceptionAsync(context, e);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        ExceptionResponse response;

        switch (exception)
        {
            case BadRequestException:
                response = new ExceptionResponse(HttpStatusCode.BadRequest, exception.Message);
                break;
            case NotFoundException:
                response = new ExceptionResponse(HttpStatusCode.NotFound, exception.Message);
                break;
            case UnauthorizedAccessException:
                response = new ExceptionResponse(HttpStatusCode.Unauthorized, exception.Message);
                break;
            default:
                response = new ExceptionResponse(HttpStatusCode.InternalServerError, exception.Message);
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)response.StatusCode;
        await context.Response.WriteAsJsonAsync(response);
    }
}