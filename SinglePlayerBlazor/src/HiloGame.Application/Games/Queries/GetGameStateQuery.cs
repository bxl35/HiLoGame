using HiloGame.Application.Common.Interfaces;
using HiloGame.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HiloGame.Application.Games.Queries
{

    public record GetGameStateQuery(Guid GameId) : IRequest<GameState?>;
    public class GetGameStateQueryHandler : IRequestHandler<GetGameStateQuery, GameState?>
    {
        private readonly IGameRepository _repository;
        public GetGameStateQueryHandler(IGameRepository repository)
        {
            _repository = repository;
        }
        public async Task<GameState?> Handle(GetGameStateQuery request, CancellationToken cancellationToken)
        {
            var state =  await _repository.GetGameByIdAsync(request.GameId, cancellationToken);
            return state;
        }

    }
}
