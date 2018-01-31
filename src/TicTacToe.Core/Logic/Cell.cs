namespace TicTacToe.Core
{
    public class Cell
    {
        public int X { get; }
        public int Y { get; }
        public IPlayer Owner { get; set; }
        public bool IsEmpty => Owner == null;

        public Cell(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool HasSameCoordinates(Cell cell) =>
            X == cell.X && Y == cell.Y;
    }
}
