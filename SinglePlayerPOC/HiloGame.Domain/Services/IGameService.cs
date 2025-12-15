using HiloGame.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HiloGame.Domain.Services;

public interface IGameService
{

    GameState InitializeGame(GameDifficulty difficulty);
    (GuessResult Result, GameState NewState) ProcessGuess(GameState gameState, int guess);

}
