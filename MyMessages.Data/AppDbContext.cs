using Microsoft.EntityFrameworkCore;
using MyMessages.Data.Entities;

namespace MyMessages.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Sticker> Stickers { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Picture>()
                .HasOne(picture => picture.Message)
                .WithOne(message => message.Picture)
                .HasForeignKey<Message>(message => message.PictureId);

            modelBuilder.Entity<File>()
                .HasOne(file => file.Message)
                .WithOne(message => message.File)
                .HasForeignKey<Message>(message => message.FileId);

            modelBuilder.Entity<Sticker>()
                .HasMany(sticker => sticker.Messages)
                .WithOne(message => message.Sticker)
                .HasForeignKey(message => message.StickerId);

            modelBuilder.Entity<User>().HasData(
                new User() { Id = 1, Name = "admin", PasswordHash = "21232f297a57a5a743894a0e4a801fc3" }
            );

            modelBuilder.Entity<Sticker>().HasData(
                new Sticker() { Id = 1, Name = "Apricot", Path = @"stickers\apricot.png" },
                new Sticker() { Id = 2, Name = "Calendula", Path = @"stickers\calendula.png" },
                new Sticker() { Id = 3, Name = "Chamomile", Path = @"stickers\chamomile.png" },
                new Sticker() { Id = 4, Name = "Cloud", Path = @"stickers\cloud.png" },
                new Sticker() { Id = 5, Name = "Flowers", Path = @"stickers\flowers.png" },
                new Sticker() { Id = 6, Name = "Heart", Path = @"stickers\heart.png" },
                new Sticker() { Id = 7, Name = "Hills", Path = @"stickers\hills.png" },
                new Sticker() { Id = 8, Name = "Mallow", Path = @"stickers\mallow.png" },
                new Sticker() { Id = 9, Name = "Mushroom", Path = @"stickers\mushroom.png" },
                new Sticker() { Id = 10, Name = "Rainbow", Path = @"stickers\rainbow.png" },
                new Sticker() { Id = 11, Name = "Sun", Path = @"stickers\sun.png" },
                new Sticker() { Id = 12, Name = "Tree", Path = @"stickers\tree.png" }
            );
        }
    }
}
