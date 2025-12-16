using HiloGame.Domain.Models;

namespace HiloGame.Application.Common.Interfaces;

public interface IGameRepository
{
    Task SaveGameAsync(GameState state, CancellationToken cancellationToken);
    Task<GameState?> GetGameByIdAsync(Guid gameId);
    Task<GameState?> GetGameByIdAsync(Guid gameId, CancellationToken cancellationToken);

}