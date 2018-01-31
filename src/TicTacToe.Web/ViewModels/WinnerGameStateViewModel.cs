using System.Collections.Generic;
using TicTacToe.Core;

namespace TicTacToe.Web
{
    public class WinnerGameStateViewModel : GameStateViewModel
    {
        public IEnumerable<CellViewModel> WinningCells { get; set; }
        public PlayerMark Winner { get; set; }
    }
}
