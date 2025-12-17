using HiloGame.Application.Common.Interfaces;
using HiloGame.Application.Games.Queries;
using HiloGame.Domain.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using static HiloGame.Domain.Enums;

namespace HiloGame.Application.Tests;

[TestFixture]
public class GetGameStateQueryHandlerTests
{
    [Test]
    public void Handle_ExistingId_ShoudReturnGameState()
    {
        // ARRANGE
        var gameId = Guid.NewGuid();
        var mockRepository = new Mock<IGameRepository>();
        var expectedState = new Domain.Models.GameState(GameDifficulty.Easy, new GameRange(1, 10), 5) with
        {
            GameId = gameId
        };
        mockRepository
            .Setup(x => x.GetGameByIdAsync(gameId, CancellationToken.None))
            .ReturnsAsync(expectedState);

        var handler = new GetGameStateQueryHandler(mockRepository.Object);
        var query = new GetGameStateQuery(gameId);

        // ACT
        var result = handler.Handle(query, CancellationToken.None).Result;

        // ASSERT
        Assert.That(result, Is.Not.Null);
        Assert.That(result?.GameId, Is.EqualTo(gameId));
    }
}
