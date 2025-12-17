using HiloGame.Domain.Models;
using HiloGame.Domain.Services;
using HiloGame.Domain.Dtos;
using MediatR;
using HiloGame.Application.Common.Interfaces;
using static HiloGame.Domain.Enums;

namespace HiloGame.Application.Games.Commands;

public record StartGameCommand(GameDifficulty Difficulty) : IRequest<GameStateDto>;

public class StartGameCommandHandler : IRequestHandler<StartGameCommand, GameStateDto>
{
    private readonly IGameService _gameService;
    private readonly IGameRepository _repository;

    public StartGameCommandHandler(IGameService gameService, IGameRepository repository)
    {
        _gameService = gameService;
        _repository = repository;

    }

    public async Task<GameStateDto> Handle(StartGameCommand request, CancellationToken cancellationToken)
    {
        var state = _gameService.InitializeGame(request.Difficulty);
        await _repository.SaveGameAsync(state, cancellationToken);
        return new GameStateDto(state.GameId,state.Difficulty,state.GuessCount,state.IsGameWon,state.IsGameOver, state.Range);
    }

}
