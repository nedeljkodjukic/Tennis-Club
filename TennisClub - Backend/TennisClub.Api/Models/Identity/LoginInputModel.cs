using System.ComponentModel.DataAnnotations;

namespace TennisClub.Api.Models.Identity;

public class LoginInputModel
{
    [Required]
    [EmailAddress]
    public string Email { get; init; }

    [Required]
    [DataType(DataType.Password)]
    [StringLength(maximumLength: 100, MinimumLength = 8, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.")]
    public string Password { get; init; }
}
