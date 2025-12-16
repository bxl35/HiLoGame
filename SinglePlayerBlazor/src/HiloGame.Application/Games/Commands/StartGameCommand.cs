using HiloGame.Domain.Models;
using HiloGame.Domain.Services;
using MediatR;
using static HiloGame.Domain.Enums;

namespace HiloGame.Application.Games.Commands;

public record StartGameCommand(GameDifficulty Difficulty) : IRequest<GameState>;

public class StartGameCommandHandler : IRequestHandler<StartGameCommand, GameState>
{
    private readonly IGameService _gameService;

    public StartGameCommandHandler(IGameService gameService)
    {
        _gameService = gameService;
    }

    public Task<GameState> Handle(StartGameCommand request, CancellationToken cancellationToken)
    {
        var state = _gameService.InitializeGame(request.Difficulty);
        return Task.FromResult(state);
    }

}
