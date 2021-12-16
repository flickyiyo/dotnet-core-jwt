using Microsoft.EntityFrameworkCore;

namespace courses_platform.Models
{
    public class CoursesDbContext : DbContext
    {

        // private IConfiguration _config;
        // public CoursesDbContext(IConfiguration config)
        // {
        //     _config = config;
        // }
        public CoursesDbContext() {}
        public CoursesDbContext(DbContextOptions<CoursesDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Course> Courses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();
            
            
            base.OnModelCreating(modelBuilder);
        }
    }
}