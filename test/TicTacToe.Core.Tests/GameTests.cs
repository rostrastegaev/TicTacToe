using Xunit;

namespace TicTacToe.Core.Tests
{
    public class GameTests
    {
        private IGame _game;

        public GameTests()
        {
            _game = new Game(new PlayersQueueFactory());
        }

        [Fact]
        public void WinCheck()
        {
            TurnInfo turn = _game.MakeTurn(new Cell(0, 0));
            Assert.False(turn.GameState.IsEnded);

            turn = _game.MakeTurn(new Cell(1, 0));
            Assert.False(turn.GameState.IsEnded);

            turn = _game.MakeTurn(new Cell(0, 1));
            Assert.False(turn.GameState.IsEnded);

            turn = _game.MakeTurn(new Cell(1, 1));
            Assert.False(turn.GameState.IsEnded);

            turn = _game.MakeTurn(new Cell(0, 2));
            Assert.True(turn.GameState.IsEnded);
        }
    }
}
