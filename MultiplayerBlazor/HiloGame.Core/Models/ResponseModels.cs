using System;
using System.Collections.Generic;
using System.Text;

namespace HiloGame.Core.Models
{
    public class CreateGameResponse: GameModel
    {

    }
    public class AvailableGamesResponse
    {
        public List<GameModel> AvailableGames = new List<GameModel>();
    }
}
