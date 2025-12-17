using System;
using System.Collections.Generic;
using System.Text;

namespace HiloGame.Domain;

public class Enums
{

    public enum GameDifficulty
    {
        Easy = 10,
        Normal = 50,
        Hard = 100,
        Expert = 1000
    }

    public enum GuessFeedback
    {
        Undefined = 0,  // safe default
        TooLow,         // represents 'HI'
        TooHigh,        // represents 'LO' 
        Correct         // set when player guessed the mystery number
    }

}
