using System.ComponentModel.DataAnnotations;

public class UserRegistrationDto
{
    public string? FirstName { get; set; }
    
    public string? LastName { get; set; }

    public string? Username { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid Email")]
    public string? Email { get; set; }
    
    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; set; }
    
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match")]
    public string? ConfirmPassword { get; set; }
}