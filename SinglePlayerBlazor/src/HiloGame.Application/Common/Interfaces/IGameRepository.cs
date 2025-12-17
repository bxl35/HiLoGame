using HiloGame.Domain.Models;

namespace HiloGame.Application.Common.Interfaces;

public interface IGameRepository
{
    Task SaveGameAsync(GameState state, CancellationToken cancellationToken);
    Task<GameState?> GetGameByIdAsync(Guid gameId, CancellationToken cancellationToken);
    Task UpdateGameAsync(GameState state, CancellationToken cancellationToken);

}