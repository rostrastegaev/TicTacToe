using System.Collections.Generic;

namespace TicTacToe.Core
{
    public class WinnerGameState : IGameState
    {
        public bool IsEnded => true;
        public IPlayer Winner { get; }
        public IEnumerable<Cell> WinningCells { get; }

        public WinnerGameState(IPlayer winner, IEnumerable<Cell> winningCells)
        {
            Winner = winner;
            WinningCells = winningCells;
        }
    }
}
