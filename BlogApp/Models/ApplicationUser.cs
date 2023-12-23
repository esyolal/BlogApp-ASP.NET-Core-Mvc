using BlogApp.Data;
using Microsoft.AspNetCore.Identity;
namespace BlogApp.Models;

public class ApplicationUser : IdentityUser
{
    public bool IsDeleted { get; set; }
    public string? FullName { get; set; }
    public string? Password { get; set; }
    public byte[]? PictureSource { get; set; }
    public List<Comments>? Comments { get; set; }
    public List<Replies>? Replies { get; set; }
    public List<Posts>? Posts { get; set; }
}
