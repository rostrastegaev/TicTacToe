namespace TicTacToe.Core
{
    public class Player : IPlayer
    {
        public PlayerMark Mark { get; }

        public Player(PlayerMark mark)
        {
            Mark = mark;
        }

        public void FillCell(Cell cell)
        {
            if (cell.IsEmpty)
            {
                cell.Owner = this;
            }
        }

        public bool Equals(IPlayer other) =>
            Mark.Equals(other.Mark);
    }
}
