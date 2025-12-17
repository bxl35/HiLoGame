using HiloGame.Domain;
using HiloGame.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HiloGame.Domain.Dtos;

public record GameStateDto(
    Guid GameId,
    Enums.GameDifficulty Difficulty,
    int GuessCount,
    bool IsGameWon,
    bool IsGameOver,
    GameRange Range
);