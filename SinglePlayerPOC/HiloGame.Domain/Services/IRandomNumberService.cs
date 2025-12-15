namespace HiloGame.Domain.Services;

public interface IRandomNumberService
{
    int GenerateMinBoundary();
    int GenerateMysteryNumber(int minBoundary, int maxBoundary);
}