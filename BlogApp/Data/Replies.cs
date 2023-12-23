using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BlogApp.Models;

namespace BlogApp.Data
{
    public class Replies
    {
        [Key]
        public int ReplyId { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; }
        [ForeignKey("User")]
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
        [ForeignKey("Comment")]
        public int CommentId { get; set; }
        public Comments? Comment { get; set; }
    }
}