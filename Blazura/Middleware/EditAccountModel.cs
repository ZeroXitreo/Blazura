using System.ComponentModel.DataAnnotations;

namespace Blazura.Middleware;

public class EditAccountModel
{
    [Display(Name = "FirstName")]
    public string? FirstName { get; set; }

    [Display(Name = "LastName")]
    public string? LastName { get; set; }

    public string FullName => $"{FirstName} {LastName}";

    [Display(Name = "PhoneNumber")]
    public string? PhoneNumber { get; set; }

    [Display(Name = "Email")]
    public string? Email { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "CurrentPassword")]
    public string? CurrentPassword { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "NewPassword")]
    public string? NewPasswordOne { get; set; }

    public string? ProfilePicture { get; set; }

    public string? UserId { get; set; }

    public bool Persistent { get; set; }
}
