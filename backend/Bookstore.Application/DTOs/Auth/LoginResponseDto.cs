namespace Bookstore.Application.DTOs.Auth;

public class LoginResponseDto
{
    public bool IsAuthSuccessfull { get; set; }

    public string? ErrorMessage { get; set; }

    public string? Token { get; set; } 
}