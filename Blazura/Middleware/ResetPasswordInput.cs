using System.ComponentModel.DataAnnotations;

namespace Blazura.Middleware;

public class ResetPasswordInput
{
    public string UserId { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; } = string.Empty;

    public string Token { get; set; } = string.Empty;
}
