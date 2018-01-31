using System.Collections.Generic;

namespace TicTacToe.Core
{
    public interface IGameStrategy
    {
        void Calculate();
        Cell GetBestCell(IEnumerable<TurnInfo> history);
    }
}
