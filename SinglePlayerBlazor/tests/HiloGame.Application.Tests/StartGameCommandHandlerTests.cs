using HiloGame.Application.Common.Interfaces;
using HiloGame.Application.Games.Commands;
using HiloGame.Domain.Models;
using HiloGame.Domain.Services;
using Moq;
using static HiloGame.Domain.Enums;

namespace HiloGame.Application.Tests;

[TestFixture]
public class StartGameCommandHandlerTests
{
    [Test]
    public async Task Handle_ValidRequest_ShouldReturnNewGameState()
    {
        // ARRANGE
        var mockGameService = new Mock<IGameService>();
        var gameId = Guid.NewGuid();
        var difficulty = GameDifficulty.Normal;
        var range = new GameRange(1, 100);
        int mysteryNumber = 50;
        var expectedState = new GameState(difficulty, range,mysteryNumber);

        mockGameService
            .Setup(x => x.InitializeGame(difficulty))
            .Returns(expectedState);
        var mockRepository = new Mock<IGameRepository>();

        var handler = new StartGameCommandHandler(mockGameService.Object, mockRepository.Object);
        var command = new StartGameCommand(difficulty);

        // ACT
        var result = await handler.Handle(command, CancellationToken.None);

        // ASSERT
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Difficulty, Is.EqualTo(difficulty));
        mockGameService.Verify(x => x.InitializeGame(difficulty), Times.Once);
    }
   
}
