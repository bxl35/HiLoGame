using HiloGame.Domain.Models;
using HiloGame.Domain.Services;
using System;

internal class GameController
{
    private readonly IConsoleIO _consoleIO;
    private readonly IGameService _gameService;
    private GameState? _currentGame;

    public GameController(IConsoleIO consoleIO, IGameService gameService)
    {
        _consoleIO = consoleIO;
        _gameService = gameService;
    }

    public void StartGame()
    {
        try
        {
            GameDifficulty difficulty = SelectDifficulty();

            _currentGame = _gameService.InitializeGame(difficulty);
            DisplayGameStart();
            PlayGame();
        }
        catch (Exception ex)
        {
            _consoleIO.WriteLine($"Error during the game: {ex.Message}");
        }
    }

    private void PlayGame()
    {

        _consoleIO.WriteLine("\nPress any key to start the Game Session, ...");
        _consoleIO.WriteLine("To exit the game session, press \"ESC\" or propose \"0\" for mystery number during the game ");
        var chrPressed = _consoleIO.ReadKey();

        if (chrPressed == ConsoleKey.Escape)
        {
            ExitGameSession(); 
            return;
        }

        if (_currentGame == null) return;

        while (!_currentGame.IsGameOver)
        {
            _consoleIO.Write("\nEnter your guess: ");
            string? input = _consoleIO.ReadLine();
            if (input == "0")
            {
                ExitGameSession(); 
                return;
            }
            if (!int.TryParse(input, out int guess))
            {
                _consoleIO.WriteLine("Invalid input. Please enter a number.");
                continue;
            }

            GuessResult result = ProcessAndUpdateState(guess);

            if (result.Feedback == GuessFeedback.Correct)
            {
                _consoleIO.WriteLine($"\nCongratulations! You've guessed the correct number {result.Guess} in {result.GuessCount} attempts!");
                return;
            }

            string message = result.Feedback switch
            {
                GuessFeedback.TooLow => "HI!",
                GuessFeedback.TooHigh => "LO!",
                _ => "Unexpected feedback."
            };

            _consoleIO.WriteLine($"{message} - Guess #{result.GuessCount}");
        }
    }

    private GuessResult ProcessAndUpdateState(int guess)
    {
        var (result, newState) = _gameService.ProcessGuess(_currentGame!, guess);
        _currentGame = newState;
        return result;
    }

    private void ExitGameSession() 
    {
        _consoleIO.WriteLine("\nExiting the game session...");
    }

    private void DisplayGameStart()
    {
        if (_currentGame == null) return;

        _consoleIO.WriteLine($"\nGame started! Difficulty: {_currentGame.Difficulty}");
        _consoleIO.WriteLine($"Guess a number in the range {_currentGame.Range}");
    }

    private GameDifficulty SelectDifficulty()
    {
        while (true)
        {
            _consoleIO.WriteLine("\nSelect Difficulty:");
            _consoleIO.WriteLine("1. Easy (Range: 10)");
            _consoleIO.WriteLine("2. Normal (Range: 50)");
            _consoleIO.WriteLine("3. Hard (Range: 100)");
            _consoleIO.WriteLine("4. Expert (Range: 1000)");
            _consoleIO.Write("Choose difficulty: ");

            string? input = _consoleIO.ReadLine();

            GameDifficulty? difficulty = input switch
            {
                "1" => GameDifficulty.Easy,
                "2" => GameDifficulty.Normal,
                "3" => GameDifficulty.Hard,
                "4" => GameDifficulty.Expert,
                _ => null
            };

            if (difficulty.HasValue)
            {
                return difficulty.Value;
            }

            _consoleIO.WriteLine("Invalid selection. Please try again.");
        }
    }
}