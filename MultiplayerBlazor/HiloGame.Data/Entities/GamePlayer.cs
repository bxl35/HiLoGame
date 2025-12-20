using System;
using System.Collections.Generic;
using System.Text;

namespace HiloGame.Data.Entities
{
   

    public class GamePlayer
    {
        public Guid Id { get; set; }
        public Guid GameId { get; set; }
        public Guid PlayerId { get; set; }
        public bool IsReady { get; set; }
        public DateTime JoinedAt { get; set; }
        public DateTime? LeftAt { get; set; }

        public Player Player { get; set; } = null!;
        public Game Game { get; set; } = null!;

    }
}
