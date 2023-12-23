using System.ComponentModel.DataAnnotations;

namespace BlogApp.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string? FullName { get; set; }

        [Required]
        public string? Username { get; set; }
        
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string? ConfirmPassword { get; set; }
    }
}


