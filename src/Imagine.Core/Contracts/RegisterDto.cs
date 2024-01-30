using System.ComponentModel.DataAnnotations;

namespace Imagine.Core.Contracts;

public class RegisterDto : IValidatableObject
{
    [Required]
    public string UserName { get; set; }
    public string Email { get; set; }
    [Required]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+{}:<>?-]).{8,15}$", ErrorMessage = "Password must be between 8 and 15 characters and contain at least one uppercase letter, one lowercase letter, one number, and one special character.")]
    public string Password { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrEmpty(UserName))
        {
            yield return new ValidationResult("Username cannot be empty");
        }
        if (string.IsNullOrEmpty(Email))
        {
            yield return new ValidationResult("Email cannot be empty");
        }

        if (string.IsNullOrEmpty(Password))
        {
            yield return new ValidationResult("Password cannot be empty");
        }
    }
}
