using HiloGame.Application.Common.Interfaces;
using HiloGame.Application.Games.Commands;
using HiloGame.Domain.Models;
using HiloGame.Domain.Services;
using Moq;
using static HiloGame.Domain.Enums;

namespace HiloGame.Application.Tests
{
    [TestFixture]
    public class ProcessGuessCommandHandlerTests
    {
        [Test]
        public async Task Handle_ValidGuess_ShouldReturnUpdatedGameState()
        {
            // ARRANGE
            var gameId = Guid.NewGuid();
            var guess = 42;

            var existingState = new GameState(GameDifficulty.Normal, new GameRange(1, 100), 50)
            {
                GameId = gameId
            };

            var expectedFeedback = new GuessResult(guess, 1, GuessFeedback.TooLow);
            var updatedState = existingState with { GuessCount = 1 };

            var mockRepo = new Mock<IGameRepository>();
            var mockService = new Mock<IGameService>();

            mockRepo.Setup(x => x.GetGameByIdAsync(gameId, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(existingState);

            mockService.Setup(x => x.ProcessGuess(existingState, guess))
                       .Returns((expectedFeedback, updatedState));

            var handler = new ProcessGuessCommandHandler(mockService.Object, mockRepo.Object);
            var command = new ProcessGuessCommand(gameId, guess);

            // ACT
            var result = await handler.Handle(command, CancellationToken.None);

            // ASSERT
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Guess, Is.EqualTo(guess));
                Assert.That(result.Feedback, Is.EqualTo(GuessFeedback.TooLow));

                // Verify the repository was asked to update the database
                mockRepo.Verify(x => x.UpdateGameAsync(updatedState, It.IsAny<CancellationToken>()), Times.Once);
            });
        }
    }
}
