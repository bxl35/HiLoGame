using HiloGame.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics.CodeAnalysis;
using static HiloGame.Core.Enums;

namespace HiloGame.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
        }

        public DbSet<Game> Games { get; set; } = null!;
        public DbSet<Player> Players { get; set; } = null!;
        public DbSet<GamePlayer> GamePlayers { get; set; } = null!;
        public DbSet<Guess> Guesses { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Player>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.UserId).IsUnique();
                entity.Property(e => e.DisplayName).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<Game>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.DisplayName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Type).IsRequired();
                entity.Property(e => e.Difficulty).IsRequired();
                entity.HasOne(e => e.Owner)
                      .WithMany(p => p.Games)
                      .HasForeignKey(e => e.OwnerPlayerId)
                      .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasMany(e => e.Players);

                entity.HasMany(e => e.Guesses)
                      .WithOne(g => g.Game)
                      .HasForeignKey(g => g.GameId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Guess>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Value).IsRequired();
                entity.Property(e => e.Result).IsRequired();
                entity.HasOne(e => e.Game)
                      .WithMany(gr => gr.Guesses)
                      .HasForeignKey(e => e.GameId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<GamePlayer>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.PlayerId).IsRequired();
                entity.Property(e => e.GameId).IsRequired();

                entity.HasOne(e => e.Game)
                      .WithMany(g => g.Players)
                      .HasForeignKey(e => e.GameId)
                      .OnDelete(DeleteBehavior.Cascade);
                
            });


        }


        [ExcludeFromCodeCoverage]
        public void MigrateAndCreateData()
        { 
            Database.Migrate();
            
            if (Games.Any())
            {
                Games.RemoveRange(Games);
                SaveChanges();
            };

            
            if (Players.Any())
            {
                Players.RemoveRange(Players);
            }

            var playerId1 = Guid.NewGuid();
            var player1 = new Player
            {
                Id = playerId1,
                UserId = playerId1.ToString(),
                DisplayName = "TestPlayer1",
            };
            
            var playerId2 = Guid.NewGuid();
            var player2 = new Player
            {
                Id = playerId2,
                UserId = playerId2.ToString(),
                DisplayName = "TestPlayer2",
            };

            var createdAt = new DateTime(2025, 12, 20, 17, 11, 15).ToUniversalTime();

            player1.Games.Add(
                new Game(
                    ownerId: player1.Id,
                    gameType: GameType.Multiplayer,
                    difficulty: DifficultyLevel.Normal,
                    mysteryNumber: 42,
                    minRange: 32,
                    maxRange: 62,
                    status: GameStatus.Cancelled,
                    displayName: $"The Hilo Challenge - Multiplayer - Normal",
                    createdAt: createdAt.AddHours(-1))
            );



            var room = new Game(
                    ownerId: player1.Id,
                    gameType: GameType.Multiplayer,
                    difficulty: DifficultyLevel.Normal,
                    mysteryNumber: 32,
                    minRange: 32,
                    maxRange: 62,
                    status: GameStatus.Completed,
                    displayName: $"The Hilo Challenge - Multiplayer - Normal",
                    createdAt: createdAt);

            room.CompletedAt = createdAt.AddMilliseconds(25000);

            room.Guesses.Add(new Guess(room.Id, player1.Id, 20, GuessResult.TooLow, createdAt.AddMilliseconds(2500)));
            room.Guesses.Add(new Guess(room.Id, player2.Id, 24, GuessResult.TooLow, createdAt.AddMilliseconds(4500)));

            room.Guesses.Add(new Guess(room.Id, player1.Id, 25, GuessResult.TooLow, createdAt.AddMilliseconds(6500)));
            room.Guesses.Add(new Guess(room.Id, player2.Id, 60, GuessResult.TooHigh, createdAt.AddMilliseconds(9500)));

            room.Guesses.Add(new Guess(room.Id, player1.Id, 55, GuessResult.TooHigh, createdAt.AddMilliseconds(12500)));
            room.Guesses.Add(new Guess(room.Id, player2.Id, 50, GuessResult.TooHigh, createdAt.AddMilliseconds(15500)));

            room.Guesses.Add(new Guess(room.Id, player1.Id, 33, GuessResult.TooHigh, createdAt.AddMilliseconds(19500)));
            room.Guesses.Add(new Guess(room.Id, player2.Id, 30, GuessResult.TooLow, createdAt.AddMilliseconds(22500)));
            room.Guesses.Add(new Guess(room.Id, player1.Id, 32, GuessResult.Correct, createdAt.AddMilliseconds(25000)));


            room.WinnerId = player1.Id;

            player1.Games.Add(room);

            room.CompletedAt = createdAt.AddMilliseconds(25000);

            Players.Add(player1);
            Players.Add(player2);

            SaveChanges();
        }
    }
}

