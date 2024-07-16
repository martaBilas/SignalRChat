using Domain;
using Microsoft.EntityFrameworkCore;

namespace DataContext;

public class ChatDataContext : DbContext
{

    public ChatDataContext(DbContextOptions<ChatDataContext> options)
        : base(options)
    {
  
    }
    public DbSet<User> Users { get; set; }
    public DbSet<Chat> Chats { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<UserChat> UsersChats { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Chat>()
            .HasKey(c => c.Id);

        modelBuilder.Entity<Chat>()
        .HasMany(c => c.Users)
        .WithMany(c => c.UserChats)
        .UsingEntity<UserChat>();

        modelBuilder.Entity<Chat>()
            .HasMany(c => c.Messages)
            .WithOne(m => m.Chat)
            .HasForeignKey(m => m.ChatId)
            .OnDelete(DeleteBehavior.Cascade); 

        modelBuilder.Entity<Chat>()
            .HasOne(c => c.UserCreator)
            .WithMany()
            .HasForeignKey(c => c.UserCreatorId)
            .OnDelete(DeleteBehavior.Restrict); 

        modelBuilder.Entity<Message>()
            .HasKey(m => m.Id);

        modelBuilder.Entity<Message>()
            .HasOne(m => m.User)
            .WithMany()
            .HasForeignKey(m => m.UserId)
            .OnDelete(DeleteBehavior.Restrict); 

        modelBuilder.Entity<Message>()
            .HasOne(m => m.Chat)
            .WithMany(c => c.Messages)
            .HasForeignKey(m => m.ChatId)
            .OnDelete(DeleteBehavior.Cascade); 

        
        modelBuilder.Entity<User>()
            .HasKey(u => u.Id);


        base.OnModelCreating(modelBuilder);
    }

}
