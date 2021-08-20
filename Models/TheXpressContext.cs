using Microsoft.EntityFrameworkCore;

namespace TheXPressGames.Models
{
    public class TheXPressContext : DbContext
    {
        public TheXPressContext(DbContextOptions options) : base(options) { }
        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserPostLike> UserPostLikes { get; set; }
    }
}