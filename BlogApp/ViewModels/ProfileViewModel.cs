using System.ComponentModel.DataAnnotations;
using BlogApp.Models;
using Microsoft.AspNetCore.Identity;

namespace BlogApp.ViewModels
{
    public class ProfileViewModel
    {
        public string? Id { get; set; }
        public string? FullName { get; set; }
        public string? Username { get; set; }
        public IFormFile? ProfilePicture { get; set; }
        public string ?ProfilePictureBase64 { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Compare("Password", ErrorMessage = "Parolalar eşleşmiyor.")]
        [DataType(DataType.Password)]
        [Display(Name = "Parolanı onayla")]
        public string? ConfirmPassword { get; set; }

    }
}