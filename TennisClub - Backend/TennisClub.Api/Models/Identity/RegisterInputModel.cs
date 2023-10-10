using System.ComponentModel.DataAnnotations;

namespace TennisClub.Api.Models.Identity;

public class RegisterInputModel
{
    [Required]
    [EmailAddress]
    public string Email { get; init; }

    [Required]
    [DataType(DataType.Password)]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
    public string Password { get; init; }

    [Required]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; init; }

    [Required]
    [StringLength(50, ErrorMessage = "The {0} must be at max {1} characters long.")]
    public string FirstName { get; init; }

    [Required]
    [StringLength(50, ErrorMessage = "The {0} must be at max {1} characters long.")]
    public string LastName { get; init; }

    [Required]
    [StringLength(30, ErrorMessage = "The {0} must be at max {1} characters long.")]
    public string UserName { get; init; }
}
