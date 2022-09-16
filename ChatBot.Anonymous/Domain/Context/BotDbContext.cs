using ChatBot.Anonymous.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatBot.Anonymous.Domain.Context
{
    public partial class BotDbContext : DbContext
    {
        public BotDbContext()
        {
        }

        public BotDbContext(DbContextOptions<BotDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ChatRoom> ChatRooms { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserSetting> UserSettings { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.HasDefaultSchema("");

            modelBuilder.Entity<ChatRoom>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.FirstUserId, e.SecondUserId })
                    .HasName("PK__ChatRoom__CC30F18F15B0B3C6");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.NumberMessagesFirstUser).HasColumnName("NumberMessagesFirstUser");
                entity.Property(e => e.NumberMessagesSecondUser).HasColumnName("NumberMessagesSecondUser");

                entity.Property(e => e.StartDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.FirstUser)
                    .WithMany(p => p.ChatRoomFirstUsers)
                    .HasForeignKey(d => d.FirstUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ChatRooms__First__5BE2A6F2");

                entity.HasOne(d => d.SecondUser)
                    .WithMany(p => p.ChatRoomSecondUsers)
                    .HasForeignKey(d => d.SecondUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ChatRooms__Secon__5CD6CB2B");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<UserSetting>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__UserSett__1788CC4C3F92F6DF");

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.HasOne(d => d.User)
                    .WithOne(p => p.UserSetting)
                    .HasForeignKey<UserSetting>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserSetti__UserI__5AEE82B9");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
