using System;
using System.Collections.Generic;
using System.Text;
using static HiloGame.Domain.Enums;

namespace HiloGame.Domain.Models;

public record GameState(
    GameDifficulty Difficulty,
    GameRange Range,
    int MysteryNumber,
    int GuessCount,
    bool IsGameWon,
    bool IsGameOver)
{
    public GameState(GameDifficulty difficulty, GameRange range, int mysteryNumber)
        : this(difficulty, range, mysteryNumber, 0, false, false)
    {
        if (!range.IsWithinRange(mysteryNumber))
        {
            throw new ArgumentException($"Mystery number must be within range {range}.", nameof(mysteryNumber));
        }
    }

}