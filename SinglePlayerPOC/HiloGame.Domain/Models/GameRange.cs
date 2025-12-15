namespace HiloGame.Domain.Models;

public class GameRange
{
    public int MinValue { get; }
    public int MaxValue { get; }

    public GameRange(int minValue, int maxValue)
    {
        if (minValue > maxValue)
        {
            throw new ArgumentException("MinValue cannot be greater than MaxValue.", nameof(minValue));
        }

        MinValue = minValue;
        MaxValue = maxValue;
    }

    public bool IsWithinRange(int value) => value >= MinValue && value <= MaxValue;

    public override string ToString() => $"[{MinValue}, {MaxValue}]";
    
}
