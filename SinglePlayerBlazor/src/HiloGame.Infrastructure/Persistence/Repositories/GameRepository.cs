using HiloGame.Application.Common.Interfaces;
using HiloGame.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace HiloGame.Infrastructure.Persistence.Repositories;

public class GameRepository : IGameRepository
{
    private readonly ApplicationDbContext _context;

    public GameRepository(ApplicationDbContext dbContext)
    {
        _context = dbContext;
    }

    public async Task<GameState?> GetGameByIdAsync(Guid gameId, CancellationToken cancellationToken)
    {
        return await _context.Games.FindAsync(new object[] { gameId }, cancellationToken);
    }

    public async Task<GameState?> GetGameByIdAsync(Guid gameId)
    {
        return await _context.Games.FindAsync(gameId);
    }

    public async Task SaveGameAsync(GameState state, CancellationToken cancellationToken)
    {
        _context.Games.Add(state);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
