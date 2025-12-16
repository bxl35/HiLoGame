using HiloGame.Domain.Models;
using HiloGame.Infrastructure.Persistence;
using HiloGame.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using static HiloGame.Domain.Enums;

namespace HiloGame.Infrastructure.Tests;

[TestFixture]
public class GameRepositoryTests
{
    [Test]
    public async Task SaveGame_ShouldPersistGameInDatabase()
    {
        // ARRANGE
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        using var context = new ApplicationDbContext(options);
        var repository = new GameRepository(context);
        int mysteryNumber = 50;
        var state = new GameState(GameDifficulty.Normal, new GameRange(1, 100), mysteryNumber);

        // ACT
        await repository.SaveGameAsync(state, CancellationToken.None);

        // ASSERT
        var savedGame = await context.Games.FirstOrDefaultAsync();
        Assert.That(savedGame, Is.Not.Null);
    }
}