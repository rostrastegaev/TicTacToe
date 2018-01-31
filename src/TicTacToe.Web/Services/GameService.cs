using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicTacToe.Core;
using TicTacToe.DataAccess;
using TicTacToe.Entities;

namespace TicTacToe.Web.Services
{
    public class GameService : IGameService
    {
        private readonly TicTacToeDbContext _dbContext;
        private readonly IGameFactory _gameFactory;
        private readonly ILogger<GameService> _logger;
        private readonly ICurrentGame _currentGame;
        private readonly IGameStrategy _gameStrategy;
        private bool _disposed;

        public GameService(TicTacToeDbContext dbContext, IGameFactory gameFactory,
            ILogger<GameService> logger, ICurrentGame currentGame, IGameStrategy gameStrategy)
        {
            _dbContext = dbContext;
            _gameFactory = gameFactory;
            _logger = logger;
            _currentGame = currentGame;
            _gameStrategy = gameStrategy;
        }

        public async Task<GameViewModel> Create(GameViewModel game)
        {
            ThrowIfDisposed();

            try
            {
                var gameEntity = new GameEntity
                {
                    IsPlayerFirst = game.IsPlayerFirst,
                    StartedAt = DateTime.UtcNow
                };
                await _dbContext.Games.AddAsync(gameEntity);
                _currentGame.GameId = gameEntity.Id;
                GameViewModel gameModel = gameEntity.ToViewModel();

                if (!gameEntity.IsPlayerFirst)
                {
                    IGame computerGame = _gameFactory.Create();
                    Cell bestCell = _gameStrategy.GetBestCell(computerGame.History);
                    computerGame.MakeTurn(bestCell);

                    await _dbContext.Turns.AddAsync(bestCell.ToEntity(gameEntity.Id));
                    gameModel.History = computerGame.History.Select(t => t.ToViewModel());
                }

                await _dbContext.SaveChangesAsync();
                return gameModel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }

        public async Task<GameViewModel> GetCurrent()
        {
            ThrowIfDisposed();

            if (_currentGame.GameId == 0)
            {
                return null;
            }

            try
            {
                GameEntity gameEntity = await _dbContext.Games.FindAsync(_currentGame.GameId);
                if (gameEntity == null || gameEntity.EndedAt.HasValue)
                {
                    return null;
                }

                return await GetGameWithHistory(gameEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }

        public async Task<FetchResult<GameViewModel>> GetGames(Fetch fetch)
        {
            ThrowIfDisposed();

            try
            {
                FetchResult<GameEntity> games = await _dbContext.Games.Where(g => g.EndedAt.HasValue)
                    .OrderByDescending(g => g.Id).Fetch(fetch);
                return new FetchResult<GameViewModel>
                {
                    Page = games.Page,
                    PagesCount = games.PagesCount,
                    TotalCount = games.TotalCount,
                    Items = games.Items.Select(g => g.ToViewModel())
                };
            }     
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }

        public async Task<GameViewModel> GetHistory(int gameId)
        {
            ThrowIfDisposed();

            try
            {
                GameEntity game = await _dbContext.Games.FindAsync(gameId);
                if (game == null || !game.EndedAt.HasValue)
                {
                    return null;
                }

                return await GetGameWithHistory(game);
            }            
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }

        private async Task<GameViewModel> GetGameWithHistory(GameEntity gameEntity)
        {
            List<TurnEntity> history = await _dbContext.Turns.GetOrderedTurnsForGame(gameEntity.Id).ToListAsync();
            var game = _gameFactory.Create();

            GameViewModel gameModel = gameEntity.ToViewModel();
            history.ForEach(t => game.MakeTurn(t.ToCell()));
            gameModel.History = game.History.Select(t => t.ToViewModel());
            return gameModel;
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
