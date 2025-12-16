using System;
using System.Collections.Generic;
using System.Text;

namespace HiloGame.Domain.Services;

public interface IRandomNumberService
{
    int GenerateMinBoundary();
    int GenerateMysteryNumber(int minBoundary, int maxBoundary);
}
