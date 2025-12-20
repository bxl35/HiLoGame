using HiloGame.Data;
using HiloGame.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using static HiloGame.Core.Enums;

namespace HiloGame.Domain.Services
{
    public class GameService : IGameService
    {
        private readonly ApplicationDbContext _context;
        private readonly IDateProvider _dateProvider;

        public GameService(ApplicationDbContext context, IDateProvider dateProvider)
        {

            _context = context;
            _dateProvider = dateProvider;
        }

        public async Task<List<Game>> GetActiveGamesAsync()
        {
            return await _context.Games
                .Where(g => g.Status == GameStatus.InProgress)
                .Include(g => g.Players.Where(p => p.LeftAt == null))
                .OrderByDescending(g => g.CreatedAt)
                .ToListAsync();
        }

        public async Task<Game> CreateGameAsync(string displayName, Guid ownerId, DifficultyLevel difficulty, GameType type)
        {
            var random = new Random();

            
            int range = difficulty switch
            {
                DifficultyLevel.Easy => 10,
                DifficultyLevel.Normal => 20,
                DifficultyLevel.Hard => 50,
                DifficultyLevel.Expert => 100,
                _ => 20
            };


            int rangeStart = random.Next(1, 1000 - range);
            int rangeEnd = rangeStart + range;
            int mysterNumber = random.Next(rangeStart, rangeEnd + 1);
            var gameId = Guid.NewGuid();

            var newGame = new Game(ownerId, type, difficulty, mysterNumber, rangeStart, rangeEnd, GameStatus.WaitingForPlayers, displayName, _dateProvider.UtcNow);
            newGame.Id = gameId;
            var player = new GamePlayer
            {
                Id = gameId,
                GameId = newGame.Id,
                PlayerId = ownerId,
                IsReady = false,
                JoinedAt = _dateProvider.UtcNow
            };
            
            newGame.Players.Add(player);
            await _context.Games.AddAsync(newGame);
            await _context.SaveChangesAsync();

            return newGame;
        }
    }

}
