using System;
using System.Collections.Generic;
using System.Text;
using static HiloGame.Domain.Enums;

namespace HiloGame.Domain.Models;

public record GameState
{
    public Guid GameId { get; init; }
    public GameDifficulty Difficulty { get; init; }
    public GameRange Range { get; init; } = null!; 
    public int MysteryNumber { get; init; }
    public int GuessCount { get; init; }
    public bool IsGameWon { get; init; }
    public bool IsGameOver { get; init; }

    private GameState() { }

    public GameState(GameDifficulty difficulty, GameRange range, int mysteryNumber)
    {
        GameId = Guid.NewGuid();
        Difficulty = difficulty;
        Range = range;
        MysteryNumber = mysteryNumber;
        GuessCount = 0;
        IsGameWon = false;
        IsGameOver = false;

        if (!range.IsWithinRange(mysteryNumber))
        {
            throw new ArgumentException($"Mystery number must be within range {range}.", nameof(mysteryNumber));
        }
    }
}