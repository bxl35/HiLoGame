using HiloGame.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HiloGame.Domain.Services
{
    public class GameService : IGameService
    {

        private GameState? _currentGame;
        private readonly Random _random = new();

        public GameState? CurrentGame => _currentGame;

        public GameState InitializeGame(GameDifficulty difficulty)
        {
            int rangeSize = (int)difficulty;
            int minBoundary = _random.Next(1, 1000);
            int maxBoundary = minBoundary + rangeSize + 1;
            int mysteryNumber = _random.Next(minBoundary, maxBoundary + 1);

            var gameRange = new GameRange(minBoundary, maxBoundary);
            _currentGame = new GameState(difficulty, gameRange, mysteryNumber);

            return _currentGame;
        }

        public GuessResult ProcessGuess(GameState gameState, int guess)
        {
            if (gameState.IsGameOver)
            {
                throw new InvalidOperationException("Game is already over. Cannot process additional guesses.");
            }

            gameState.IncrementGuessCount();

            if (guess == gameState.MysteryNumber)
            {
                gameState.SetGameOver(true);
                return new GuessResult(guess, gameState.GuessCount, "Congrats! You win!", true);
            }

            string feedback = guess < gameState.MysteryNumber ? "LO" : "HI";
            return new GuessResult(guess, gameState.GuessCount, feedback, false);
        }
    }
}
