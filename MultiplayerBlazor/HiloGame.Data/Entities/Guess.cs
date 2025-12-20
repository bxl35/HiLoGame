using static HiloGame.Core.Enums;

namespace HiloGame.Data.Entities
{
    public class Guess
    {
        public Guid Id { get; set; }
        public Guid GameId { get; set; }
        public Guid PlayerId { get; set; }
        public int Value { get; set; }
        public GuessResult Result { get; set; } = GuessResult.Unknown;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Game Game { get; set; } = null!;

        private Guess() { }

        public Guess(
            Guid gameId,
            Guid playerId,
            int value,
            GuessResult result,
            DateTime createdAt
            )
        {
            GameId = gameId;
            PlayerId = playerId;
            Value = value;
            Result = result;
            CreatedAt = createdAt;
        }


    }
}