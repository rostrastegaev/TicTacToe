using System.Linq;

namespace TicTacToe.Core
{
    public partial class Game
    {
        private class DrawRule : IGameEndingRule
        {
            private readonly Game _game;

            public DrawRule(Game game)
            {
                _game = game;
            }

            public IGameState Check(Cell cell)
            {
                if (!_game.GetAvailableCells().Any())
                {
                    return new DrawGameState();
                }

                return new NotEndedGameState();
            }
        }
    }
}
