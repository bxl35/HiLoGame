using HiloGame.Core;
using HiloGame.Data.Entities;

namespace HiloGame.Domain.Services
{
    public interface IGameService
    {
        Task<Game> CreateGameAsync(string displayName, Guid ownerId, Enums.DifficultyLevel difficulty, Enums.GameType type);
        Task<List<Game>> GetActiveGamesAsync();
    }
}