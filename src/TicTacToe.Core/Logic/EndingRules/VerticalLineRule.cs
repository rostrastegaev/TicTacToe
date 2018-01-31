namespace TicTacToe.Core
{
    public partial class Game
    {
        private class VerticalLineRule : WinnerEndingRule
        {
            public VerticalLineRule(Game game) : base(game)
            { }

            protected override bool CheckX(int x) =>
                x < GameConstants.TABLE_SIZE;

            protected override bool CheckY(int y) =>
                true;

            protected override int IterateX(int x) =>
                ++x;

            protected override int IterateY(int y) =>
                y;

            protected override (int x, int y) GetInitialCoordinates(Cell cell) =>
                (0, cell.Y);
        }
    }
}
