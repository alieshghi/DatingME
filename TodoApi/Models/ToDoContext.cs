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
        public DbSet<Message> Messages {get;set;}
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
        
        modelBuilder.Entity<Message>()
        .HasOne(u => u.Recipient)
        .WithMany(u => u.MessagesReceived)       
        .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Message>()
        .HasOne(u => u.Sender)
        .WithMany(u => u.MessagesSent)       
        .OnDelete(DeleteBehavior.Restrict);

        
        }
        
    }
    
}