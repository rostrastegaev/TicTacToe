using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace TicTacToe.Core.Tests
{
    public class StrategyTests
    {
        private static readonly IPlayersQueueFactory _playersFactory = new PlayersQueueFactory();
        private static readonly IGameStrategy _strategy;
        private readonly IGame _game;

        static StrategyTests()
        {
            _strategy = new GameStrategy(new GameFactory(_playersFactory));
            _strategy.Calculate();
        }

        public StrategyTests()
        {
            _game = new Game(_playersFactory);
        }

        [Fact]
        public void BestCellTest()
        {
            _game.MakeTurn(new Cell(1, 1));

            var cell = _strategy.GetBestCell(_game.History);
            _game.MakeTurn(cell);
            Assert.True(cell.HasSameCoordinates(new Cell(0, 0)));

            _game.MakeTurn(new Cell(1, 2));

            cell = _strategy.GetBestCell(_game.History);
            _game.MakeTurn(cell);
            Assert.True(cell.HasSameCoordinates(new Cell(1, 0)));
        }
    }
}
