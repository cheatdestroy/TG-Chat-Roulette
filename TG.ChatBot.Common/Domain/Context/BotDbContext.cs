﻿using TG.ChatBot.Common.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace TG.ChatBot.Common.Domain.Context
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
        public virtual DbSet<ActionData> Actions { get; set; } = null!;
        public virtual DbSet<IgnoreUsers> IgnoreUsers { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IgnoreUsers>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.IgnoredUserId });

                entity.HasOne(d => d.User)
                    .WithMany(p => p.IgnoredUsersOwner)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.IgnoredUser)
                    .WithMany(p => p.IgnoredUsersTarget)
                    .HasForeignKey(d => d.IgnoredUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<ChatRoom>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.FirstUserId, e.SecondUserId });

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.NumberMessagesFirstUser).HasColumnName("NumberMessagesFirstUser");
                entity.Property(e => e.NumberMessagesSecondUser).HasColumnName("NumberMessagesSecondUser");

                entity.Property(e => e.StartDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.HasOne(d => d.FirstUser)
                    .WithMany(p => p.ChatRoomFirstUsers)
                    .HasForeignKey(d => d.FirstUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.SecondUser)
                    .WithMany(p => p.ChatRoomSecondUsers)
                    .HasForeignKey(d => d.SecondUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
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
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.HasOne(d => d.User)
                    .WithOne(p => p.UserSetting)
                    .HasForeignKey<UserSetting>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<ActionData>(entity =>
            {
                entity.ToTable("ActionData");

                entity.HasKey(e => new { e.UserId, e.ChatId });

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.Property(e => e.LastUpdate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.Action)
                    .HasForeignKey<ActionData>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
