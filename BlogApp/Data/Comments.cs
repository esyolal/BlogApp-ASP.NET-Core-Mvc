using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BlogApp.Models;

namespace BlogApp.Data
{
    public class Comments
    {
        [Key]
        public int CommentId { get; set; }

        public string? Content { get; set; }

        public DateTime CreatedAt { get; set; }
        [ForeignKey("User")]
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public byte[]? UserPictureSource { get; set; }
        public ApplicationUser? User { get; set; }

        [ForeignKey("Post")]
        public int PostId { get; set; }
        public Posts? Post { get; set; }
    }
}
