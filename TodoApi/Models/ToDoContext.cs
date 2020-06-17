using Microsoft.EntityFrameworkCore;
namespace TodoApi.Models
{
    public class ToDoContext : DbContext
    {
        public ToDoContext(DbContextOptions<ToDoContext> options)
            : base(options)
        {
        }

        public DbSet<ToDoItems> TodoItems { get; set; }
        public DbSet<User> Users {get;set;}
        public DbSet<Photo> photos {get;set;}
        public DbSet<Like> Likes {get;set;}
        protected  override void OnModelCreating(ModelBuilder modelBuilder){
        modelBuilder.Entity<Like>().HasKey(
            x => new {x.LikerId, x.LikedId });

        modelBuilder.Entity<Like>()
        .HasOne(u => u.Liked)
        .WithMany(u => u.Likers)
        .HasForeignKey(u => u.LikedId)
        .OnDelete( DeleteBehavior.Restrict);

        modelBuilder.Entity<Like>()
        .HasOne(u => u.Liker)
        .WithMany(u => u.Likeds)
        .HasForeignKey(u => u.LikerId)
        .OnDelete( DeleteBehavior.Restrict);
        }
    }
    
}