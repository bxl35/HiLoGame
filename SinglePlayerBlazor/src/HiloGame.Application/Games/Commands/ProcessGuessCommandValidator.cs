using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HiloGame.Application.Games.Commands;

public class ProcessGuessCommandValidator : AbstractValidator<ProcessGuessCommand>
{
    public ProcessGuessCommandValidator()
    {
        RuleFor(x => x.GameId)
            .NotEmpty().WithMessage("GameId is required.");

        RuleFor(x => x.Guess)
            .InclusiveBetween(1, 1000) // General range, It should come from configuration
            .WithMessage("Guess must be between 1 and 1000.");
    }
}