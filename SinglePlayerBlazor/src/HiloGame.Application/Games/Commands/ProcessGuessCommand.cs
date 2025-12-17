using HiloGame.Application.Common.Interfaces;
using HiloGame.Domain.Models;
using HiloGame.Domain.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HiloGame.Application.Games.Commands;

public record ProcessGuessCommand(Guid GameId, int Guess) : IRequest<GuessResult>;
public class ProcessGuessCommandHandler : IRequestHandler<ProcessGuessCommand, GuessResult>
{
    private readonly IGameService _gameService;
    private readonly IGameRepository _gameRepository;

    public ProcessGuessCommandHandler(IGameService gameService, IGameRepository gameRepository)
    {
        _gameService = gameService;
        _gameRepository = gameRepository;
    }
    public async Task<GuessResult> Handle(ProcessGuessCommand request, CancellationToken cancellationToken)
    {
        var gameState = await _gameRepository.GetGameByIdAsync(request.GameId, cancellationToken);
        if (gameState == null)
        {

            //TODO: Create custom exception
            throw new InvalidOperationException("Game not found."); 
        }

        var (result, newState) = _gameService.ProcessGuess(gameState, request.Guess);
        await _gameRepository.UpdateGameAsync(newState, cancellationToken);

        return result; 

    }
}
