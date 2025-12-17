using System;
using System.Collections.Generic;
using System.Text;
using static HiloGame.Domain.Enums;

namespace HiloGame.Domain.Models;

public record GuessResult(int Guess, int GuessCount, GuessFeedback Feedback);