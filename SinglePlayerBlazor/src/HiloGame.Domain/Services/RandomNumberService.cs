using System;
using System.Collections.Generic;
using System.Text;

namespace HiloGame.Domain.Services;

public class RandomNumberService : IRandomNumberService
{
    private readonly Random _random = new();
    public int GenerateMinBoundary()
    {
        return _random.Next(1, 1000);
    }
    public int GenerateMysteryNumber(int minBoundary, int maxBoundary)
    {
        return _random.Next(minBoundary, maxBoundary);
    }

}