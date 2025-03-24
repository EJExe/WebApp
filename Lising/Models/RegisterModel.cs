using System.ComponentModel.DataAnnotations;

public class RegisterModel
{
    [Required(ErrorMessage = "Username is required")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }

    //public string Role { get; set; }
    [Required]
    public string Role { get; set; } = "Client";
}