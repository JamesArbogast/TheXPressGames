using System;
using System.ComponentModel.DataAnnotations;

namespace TheXPressGames.Models
{
    public class UserPostLike
    {
        [Key]
        public int UserPostLikeId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        //foreign keys and navigational properties
        public int UserId { get; set; } // FK
        public User User { get; set; } // NG
        public int PostId { get; set; } // Fk
        public Post Post { get; set; } // NG
    }
}