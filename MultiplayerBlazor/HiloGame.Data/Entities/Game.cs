using static HiloGame.Core.Enums;

namespace HiloGame.Data.Entities
{
    public class Game
    {
        public Guid Id { get; set; }

        public string DisplayName { get; set; } = string.Empty;

        public GameType Type { get; set; } = GameType.Unknown;
        public Guid OwnerPlayerId { get; set; }
        public DifficultyLevel Difficulty { get; set; } = DifficultyLevel.Normal;
        public int MysteryNumber { get; set; }
        public int MinRange { get; set; }
        public int MaxRange { get; set; }

        public GameStatus Status { get; set; } = GameStatus.WaitingForPlayers;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }

        public Guid WinnerId { get; set; }

        public int GuessAttempts { get; set; }

        public Player Owner { get; set; }
        public ICollection<Player> Players { get; set; } = new List<Player>();

        public ICollection<Guess> Guesses { get; set; } = new List<Guess>();

        private Game() { }
        public Game(
            Guid ownerId,
            GameType gameType, 
            DifficultyLevel difficulty, 
            int mysteryNumber, 
            int minRange,
            int maxRange,
            GameStatus status,
            string displayName, 
            DateTime createdAt
            )
        {
            Type = gameType;
            Difficulty = difficulty;
            MysteryNumber = mysteryNumber;
            MinRange = minRange;
            MaxRange = maxRange;
            Status = status;
            OwnerPlayerId = ownerId;
            DisplayName = displayName;
            CreatedAt = createdAt;
        }
    }
}
