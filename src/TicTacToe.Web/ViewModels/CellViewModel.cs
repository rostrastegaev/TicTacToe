using TicTacToe.Core;

namespace TicTacToe.Web
{
    public class CellViewModel
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Cell ToCell() =>
            new Cell(X, Y);
    }
}
