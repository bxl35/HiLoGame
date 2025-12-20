using HiloGame.Core.Models;
using HiloGame.Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace HiloGame.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IGameService _gameService;
        public GamesController(IGameService gameService)
        {
            _gameService = gameService;
        }


        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableGames()
        {
            var games = await _gameService.GetActiveGamesAsync();

            var response = new AvailableGamesResponse();

            foreach (var game in games)
            {
                var gameModel = new GameModel
                {
                    GameId = game.Id.ToString(),
                    DisplayName = game.DisplayName,
                    PlayerCount = game.Players.Count,
                    DifficultyLevel = game.Difficulty,
                    OwnerId = game.OwnerPlayerId.ToString(),
                    Status = game.Status,
                    MinRange = game.MinRange,
                    MaxRange = game.MaxRange
                };
                response.AvailableGames.Add(gameModel);
            }

            return Ok(response);
        }

        private string GetUserName()
        {
            var name = User.FindFirst(ClaimTypes.Name)?.Value
                ?? User.FindFirst("name")?.Value;

            if (string.IsNullOrEmpty(name))
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
                return $"User {userId ?? "Unknown"}";
            }

            return name;
        }

        private string GetUserId()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException("User ID not found in claims");
            }
            return userId;
        }

    }
}
