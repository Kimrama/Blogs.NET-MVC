using Microsoft.EntityFrameworkCore;
using blog.Models;

namespace blog.Data {
    public class AppDbContext : DbContext {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
        public DbSet<Blog> Blogs { get; set; } = null!;
        public DbSet<User> Users{ get; set; } = null!;

    }
}