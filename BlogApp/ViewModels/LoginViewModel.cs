using System.ComponentModel.DataAnnotations;
namespace BlogApp.ViewModels
{
    public class LoginViewModel
    {
        public int id {get;set;}
        [Required]
        public string? Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }

}