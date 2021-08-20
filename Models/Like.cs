using System;
using System.ComponentModel.DataAnnotations;

namespace TheXPressGames.Models
{
    public class Like // Many To Many 'Through' / 'Join' Table
    {
        [Key]
        public int LikeId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // foreign keys and navigational
        public int UserId { get; set; } // FK
        public User User { get; set; }
        public int PostId { get; set; } // Fk
        public Post Post { get; set; }
    }
}