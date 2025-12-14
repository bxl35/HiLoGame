using System;
using System.Collections.Generic;
using System.Text;

namespace HiloGame.Domain.Models
{
    public class GameState
    {
        public GameDifficulty Difficulty { get;  }
        public GameRange Range { get; }
        public int MysteryNumber { get; }
        public int GuessCount { get; private set; }
        public bool IsGameWon { get; private set; }
        public int CurrentNumber { get;  }

        //this can be used to introduce new game time with implicit max attempts based on difficulty
        public int RemainingAttempts { get; }
        public bool IsGameOver { get; private set; }

        public GameState(GameDifficulty difficulty, GameRange range, int mysteryNumber)
        {
            if (!range.IsWithinRange(mysteryNumber))
            {
                throw new ArgumentException($"Mystery number must be within range {range}.", nameof(mysteryNumber));
            }

            Difficulty = difficulty;
            Range = range;
            MysteryNumber = mysteryNumber;
            GuessCount = 0;
            IsGameOver = false;
            IsGameWon = false;
        }

        public void IncrementGuessCount() => GuessCount++;

        public void SetGameOver(bool won)
        {
            IsGameOver = true;
            IsGameWon = won;
        }

    }
}
