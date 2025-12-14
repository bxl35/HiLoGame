using System;
using System.Collections.Generic;
using System.Text;

namespace HiLoGame.SinglePlayerPOC.Console
{
    internal class ApplicationController
    {
        private readonly IConsoleIO _consoleIO;
        private readonly GameController _gameController;
        private bool _isApplicationRunning;

        public ApplicationController(IConsoleIO consoleIO, GameController gameController)
        {
            _consoleIO = consoleIO ?? throw new ArgumentNullException(nameof(consoleIO));
            _gameController = gameController ?? throw new ArgumentNullException(nameof(gameController));
        }

        public void Run()
        {
            try
            {
                _isApplicationRunning = true;
                DisplayWelcomeMessage();

                while (_isApplicationRunning)
                {
                    DisplayMainMenu();
                    ProcessMainMenuInput();
                }

                DisplayGoodbyeMessage();
            }
            catch (Exception ex)
            {
                _consoleIO.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }

        private void DisplayGoodbyeMessage()
        {
            throw new NotImplementedException();
        }

        private void ProcessMainMenuInput()
        {
            string? userInput = _consoleIO.ReadLine();

            try
            {
                switch (userInput)
                {
                    case "1":
                        _gameController.StarGame();
                        break;
                    case "2":
                        _isApplicationRunning = false;
                        break;
                    default:
                        _consoleIO.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
            catch (Exception ex)
            {
                _consoleIO.WriteLine($"Error processing menu selection: {ex.Message}");
            }
        }

        private void DisplayMainMenu()
        {
            _consoleIO.WriteLine("\nMain Menu:");
            _consoleIO.WriteLine("1. Start New Game");
            _consoleIO.WriteLine("2. Exit");
            _consoleIO.Write("Please select an option: ");
        }

        private void DisplayWelcomeMessage()
        {
            _consoleIO.WriteLine("########  Welcome to Hi-Lo Game  ########");
        }
    }
}
