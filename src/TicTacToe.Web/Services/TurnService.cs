using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicTacToe.Core;
using TicTacToe.DataAccess;
using TicTacToe.Entities;

namespace TicTacToe.Web.Services
{
    public class TurnService : ITurnService
    {
        private readonly IGameFactory _gameFactory;
        private readonly TicTacToeDbContext _dbContext;
        private readonly IGameStrategy _gameStrategy;
        private readonly ILogger<TurnService> _logger;
        private readonly ICurrentGame _currentGame;
        private bool _disposed;

        public TurnService(IGameFactory gameFactory, TicTacToeDbContext dbContext,
            IGameStrategy gameStrategy, ILogger<TurnService> logger, ICurrentGame currentGame)
        {
            _gameFactory = gameFactory;
            _dbContext = dbContext;
            _gameStrategy = gameStrategy;
            _logger = logger;
            _currentGame = currentGame;
        }

        public async Task<TurnInfoViewModel> MakeTurn(CellViewModel cell)
        {
            ThrowIfDisposed();

            if (_currentGame.GameId == 0)
            {
                return null;
            }

            try
            {
                GameEntity gameEntity = await _dbContext.Games.FindAsync(_currentGame.GameId);
                if (gameEntity == null)
                {
                    return null;
                }

                List<TurnEntity> history = await _dbContext.Turns.GetOrderedTurnsForGame(gameEntity.Id).ToListAsync();
                IGame game = _gameFactory.Create();
                history.ForEach(t => game.MakeTurn(t.ToCell()));

                TurnInfo turnInfo = game.MakeTurn(cell.ToCell());
                await _dbContext.Turns.AddAsync(turnInfo.Cell.ToEntity(gameEntity.Id));
                if (turnInfo.GameState.IsEnded)
                {
                    EndGame(gameEntity);
                    await _dbContext.SaveChangesAsync();
                    return turnInfo.ToViewModel();
                }

                Cell nextBestCell = _gameStrategy.GetBestCell(game.History);
                TurnInfo computerTurn = game.MakeTurn(nextBestCell);
                await _dbContext.Turns.AddAsync(computerTurn.Cell.ToEntity(gameEntity.Id));
                if (computerTurn.GameState.IsEnded)
                {
                    EndGame(gameEntity);
                }

                await _dbContext.SaveChangesAsync();
                return computerTurn.ToViewModel();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }

        private void EndGame(GameEntity game)
        {
            game.EndedAt = DateTime.UtcNow;
            _dbContext.Games.Update(game);
            _currentGame.GameId = 0;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _dbContext.Dispose();
            }

            _disposed = true;
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(GameService));
            }
        }
    }
}
