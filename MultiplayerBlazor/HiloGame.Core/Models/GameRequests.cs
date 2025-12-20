
using static HiloGame.Core.Enums;

namespace HiloGame.Core.Models
{

    
    public class NewGameRequestModel
    {
        public string Display { get; set; } = string.Empty;
        public DifficultyLevel Difficulty { get; set; }
        public GameType GameType { get; set; }
    }
       

    public abstract class BaseGameRequest
    {
        public Guid GameId { get; set; }
        public Guid PlayerId { get; set; }
    }

    public class JoinGameRequest : BaseGameRequest { }
    public class ReadyRequest : BaseGameRequest{ }
    public class StartGameRequest : BaseGameRequest{ }
    public class LeaveGameRequest : BaseGameRequest{ }
    public class MakeGuessRequest : BaseGameRequest
    {
        public int GuessNumber { get; set; }
    }
}
