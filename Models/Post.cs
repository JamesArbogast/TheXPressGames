using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TheXPressGames.Models
{
    public class Post
    {
        [Key] // Primary Key
        public int PostId { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Must be more than 2 characters.")]
        [MaxLength(45, ErrorMessage = "Must be less than 45 characters.")]
        public string Topic { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Must be more than 2 characters.")]
        public string Body { get; set; }

        [Display(Name = "Image Url")]
        public string ImgUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        /* 
        Foreign Keys and Navigation Properties for Relationships
        Navigation properties are null unless you use .Include / .ThenInclude
        */
        public int UserId { get; set; } // FK 1 User : Many Post
        // Navigation Property for 1 User : Many Post
        public User Author { get; set; }
        public List<UserPostLike> Likes { get; set; } // 1 user can like many posts
    }
}