using System.ComponentModel.DataAnnotations;

public class ForgotPasswordVM
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}
