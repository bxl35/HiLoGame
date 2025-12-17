using Microsoft.AspNetCore.Mvc;
using MediatR;
using HiloGame.Domain.Dtos;
using HiloGame.Application.Games.Commands;
using HiloGame.Application.Games.Queries;
using HiloGame.Infrastructure.Persistence;

namespace HiloGame.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {

        private readonly IMediator _mediator;

        public GamesController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult<GameStateDto>> StartGame([FromBody] StartGameCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetState(Guid id)
        {
            var result = await _mediator.Send(new GetGameStateQuery(id));
            return result != null ? Ok(result) : NotFound();
        }

        [HttpPost("{id}/guess")]
        public async Task<IActionResult> Guess(Guid id, [FromBody] int guess)
        {
            var result = await _mediator.Send(new ProcessGuessCommand(id, guess));
            return Ok(result);
        }
    }
}
