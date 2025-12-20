namespace HiloGame.Core
{
    public class Enums
    {
        public enum DifficultyLevel
        {
            Easy,
            Normal,
            Hard,
            Expert
        }

        public enum GameStatus
        {
            WaitingForPlayers,
            InProgress,
            Completed,
            Cancelled
        }

        public enum GuessResult
        {
            Unknown,
            TooLow,
            TooHigh,
            Correct
        }
        public enum GameType
        {
            Unknown,
            SinglePlayer,
            Multiplayer
            
        }
    }
}
