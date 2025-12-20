namespace HiloGame.Data.Entities
{
    public class Player
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string? DisplayName { get; set; } = "No Name";

        public ICollection<Game> Games { get; set; } = new List<Game>();
        public ICollection<Guess> Guesses { get; set; } = new List<Guess>();
    }
}
