namespace HiloGame.Domain.Models
{
    public record GuessResult(int Guess, int GuessCount, string Feedback, bool IsCorrect);
}
