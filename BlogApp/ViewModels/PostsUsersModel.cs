using System.ComponentModel.DataAnnotations;
using BlogApp.Data;
using BlogApp.Models;

namespace BlogApp.ViewModels
{
    public class PostsUsersModel
    {
        public List<Posts>? AllPosts { get; set; }

        public ApplicationUser? User { get; set; }

        public List<Comments>? Comments { get; set; }
        public Comments? Comment { get; set; }
        public Posts? Post { get; set; }
    }
}
