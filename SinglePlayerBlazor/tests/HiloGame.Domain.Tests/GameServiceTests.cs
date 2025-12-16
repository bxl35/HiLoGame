using HiloGame.Domain.Models;
using HiloGame.Domain.Services;
using static HiloGame.Domain.Enums;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace HiloGame.Domain.Tests;

[TestFixture]
public class GameServiceTests
{

    private GameService? _gameService;
    private Mock<IRandomNumberService>? _mockRng;

    [SetUp]
    public void Setup()
    {
        _mockRng = new Mock<IRandomNumberService>();
        _gameService = new GameService(_mockRng.Object);
    }

    [Test]
    public void InitializeGame_CreatesStateWithinExpectedRange()
    {
        // Arrange
        int expectedMin = 50;
        int expectedMysteryNumber = 55;
        GameDifficulty difficulty = GameDifficulty.Easy; 

        _mockRng!.Setup(rng => rng.GenerateMinBoundary()).Returns(expectedMin);

        
        var intendedMaxValue = expectedMin + (int)difficulty;

        var rngExclusiveMax = intendedMaxValue + 1;
        _mockRng.Setup(rng => rng.GenerateMysteryNumber(expectedMin, rngExclusiveMax)).Returns(expectedMysteryNumber);

        // Act
        GameState initialState = _gameService!.InitializeGame(difficulty);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(initialState.MysteryNumber, Is.EqualTo(expectedMysteryNumber), $"Mystery number must be equal to {expectedMysteryNumber}.");
            Assert.That(initialState.Range.MinValue, Is.EqualTo(expectedMin), "Range.min must be set");
            Assert.That(initialState.Range.MaxValue, Is.EqualTo(intendedMaxValue), $"Range.max must be range.min + difficulty ({intendedMaxValue}).");
            Assert.That(initialState.GuessCount, Is.EqualTo(0), "Initial guess count must be 0.");
        });
    }

    [Test]
    public void ProcessGuess_CorrectGuess_ReturnsCorrectFeedbackAndUpdatesState()
    {
        // Arrange
        int correctGuess = 42;
        GameState initialState = new GameState(
          GameDifficulty.Normal,
          new GameRange(1, 100),
          mysteryNumber: correctGuess
        ) with
        {
            GuessCount = 5 
        };

        // Act
        var (result, newState) = _gameService!.ProcessGuess(initialState, correctGuess);

        // Assert
        Assert.Multiple(() =>
        {
            
            Assert.That(result.Feedback, Is.EqualTo(GuessFeedback.Correct), "Feedback should be GuessFeedback.Correct.");
            Assert.That(result.GuessCount, Is.EqualTo(6), "Guess count in result should be (5 -> 6).");

             
            Assert.That(newState.IsGameOver, Is.True, "New state should be Game Over.");
            Assert.That(newState.IsGameWon, Is.True, "New state should be Game Won.");
            Assert.That(newState.GuessCount, Is.EqualTo(6), "New state GuessCount should be incremented.");

            
            Assert.That(initialState.GuessCount, Is.EqualTo(5), "Original state must remain unchanged.");
        });
    }

 
    [TestCase(50, GuessFeedback.TooHigh, "50 should be High (LO) when target is 42.")]
    [TestCase(30, GuessFeedback.TooLow, "30 should be Low (HI) when target is 42.")]
    public void ProcessGuess_IncorrectGuess_ReturnsCorrectFeedback(int guess, GuessFeedback expectedFeedback, string message)
    {
        // Arrange
        int mysteryNumber = 42;
        GameState initialState = new GameState(
          GameDifficulty.Normal,
          new GameRange(1, 100),
          mysteryNumber: mysteryNumber
        ) with
        {
            GuessCount = 1
        };

        // Act
        var (result, newState) = _gameService!.ProcessGuess(initialState, guess);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Feedback, Is.EqualTo(expectedFeedback), message);
            Assert.That(result.GuessCount, Is.EqualTo(2), "Guess count should be incremented (1 -> 2).");
            Assert.That(newState.IsGameOver, Is.False, "Game should not be over.");
            Assert.That(initialState.GuessCount, Is.EqualTo(1), "Original state must be immutable.");
        });
    }

    [Test]
    public void ProcessGuess_ThrowsException_IfGameIsAlreadyOver()
    {
        // Arrange
        GameState finishedState = new GameState(
      GameDifficulty.Easy,
      new GameRange(1, 10),
      mysteryNumber: 5
    ) with
        {
            GuessCount = 3,
            IsGameWon = true,
            IsGameOver = true 
        };

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
        {
            _gameService!.ProcessGuess(finishedState, 1);
        }, "Should not allow processing guesses anymore when the game is over.");
    }
}
