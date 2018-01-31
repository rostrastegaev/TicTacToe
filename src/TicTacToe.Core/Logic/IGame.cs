using System.Collections.Generic;

namespace TicTacToe.Core
{
    public interface IGame
    {
        IEnumerable<TurnInfo> History { get; }

        TurnInfo MakeTurn(Cell cell);
        IEnumerable<Cell> GetAvailableCells();
        IGame Clone();
    }
}
