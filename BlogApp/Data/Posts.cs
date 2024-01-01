using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BlogApp.Models;

namespace BlogApp.Data
{
    public class Posts
    {
        [Key]
        public int PostId { get; set; }
        public string? Title { get; set; }

        [MaxLength(200)]
        public string? Content { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<Comments>? Comments { get; set; }

        [ForeignKey("User")]
        public string? UserId { get; set; }

        public int Likes { get; set; }
        public List<string>? LikedBy { get; set; }
        public string? UserName { get; set; }
        public byte[]? PictureSource { get; set; }

        public byte[]? PostPicture { get; set; }


        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }
    }
}
