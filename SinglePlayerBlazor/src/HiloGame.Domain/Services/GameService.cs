using HiloGame.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using static HiloGame.Domain.Enums;

namespace HiloGame.Domain.Services;

public class GameService : IGameService
{

    private readonly IRandomNumberService _rng;

    public GameService(IRandomNumberService randomNumberGenerator)
    {
        _rng = randomNumberGenerator;
    }

    public GameState InitializeGame(GameDifficulty difficulty)
    {
        int rangeSize = (int)difficulty;
        int minBoundary = _rng.GenerateMinBoundary();

        // The maximum number the player is allowed to guess 
        int intendedMaxValue = minBoundary + rangeSize;

        // Upper bound for the random number generator 
        int rngExclusiveMax = intendedMaxValue + 1;

        int mysteryNumber = _rng.GenerateMysteryNumber(minBoundary, rngExclusiveMax);

        // GameRange is created with the intended MaxValue ([50, 60])
        var gameRange = new GameRange(minBoundary, intendedMaxValue);
        var newState = new GameState(difficulty, gameRange, mysteryNumber);

        return newState;
    }

    public (GuessResult Result, GameState NewState) ProcessGuess(GameState gameState, int guess)
    {
        if (gameState.IsGameOver)
        {
            throw new InvalidOperationException("Game is already over. Cannot process additional guesses.");
        }

        GameState updatedState = gameState with
        {
            GuessCount = gameState.GuessCount + 1
        };


        GuessFeedback feedback;
        if (guess == gameState.MysteryNumber)
        {
            updatedState = updatedState with
            {
                IsGameOver = true,
                IsGameWon = true
            };
            feedback = GuessFeedback.Correct;
        }
        else
        {
            feedback = guess < gameState.MysteryNumber ? GuessFeedback.TooLow : GuessFeedback.TooHigh;
        }
        var guessResult = new GuessResult(guess, updatedState.GuessCount, feedback);
        return (guessResult, updatedState);
    }
}
