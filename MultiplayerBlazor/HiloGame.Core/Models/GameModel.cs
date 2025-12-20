using System;
using System.Collections.Generic;
using System.Text;
using static HiloGame.Core.Enums;

namespace HiloGame.Core.Models
{
    

    public class GameModel
    {
        public string GameId { get; set; }
        public string DisplayName { get; set; } = string.Empty;
        public int PlayerCount { get; set; }
        public DifficultyLevel DifficultyLevel { get; set; } = DifficultyLevel.Normal;
        public string OwnerId { get; set; } = string.Empty;
        public GameStatus Status { get; set; }
        public int MinRange { get; set; }
        public int MaxRange { get; set; }

    }
}
