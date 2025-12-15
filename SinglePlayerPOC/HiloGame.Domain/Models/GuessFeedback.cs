using System;
using System.Collections.Generic;
using System.Text;

namespace HiloGame.Domain.Models;

public enum GuessFeedback
{
    Undefined = 0,  // safe default
    TooLow,         // represents 'HI'
    TooHigh,        // represents 'LO' 
    Correct         // set when player guessed the mystery number
}