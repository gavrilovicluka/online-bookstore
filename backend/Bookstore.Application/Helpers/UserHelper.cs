using System.Security.Claims;
using Bookstore.Domain.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Bookstore.Application.Helpers;

public static class UserHelper
{
    public static string? GetUserIdFromClaim(IHttpContextAccessor httpContextAccessor)
    {
        if (httpContextAccessor == null)
        {
            throw new BadRequestException("IHttpContextAccessor is null");
        }
        
        return httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
    
    public static ClaimsPrincipal GetUserClaimsPrincipal(IHttpContextAccessor httpContextAccessor)
    {
        if (httpContextAccessor == null)
        {
            throw new BadRequestException("IHttpContextAccessor is null");
        }
        
        return httpContextAccessor.HttpContext?.User ?? new ClaimsPrincipal();
    }
}