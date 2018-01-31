namespace TicTacToe.Core
{
    public partial class Game
    {
        private class HorizontalLineRule : WinnerEndingRule
        {
            public HorizontalLineRule(Game game) : base(game)
            { }

            protected override bool CheckX(int x) =>
                true;

            protected override bool CheckY(int y) =>
                y < GameConstants.TABLE_SIZE;

            protected override int IterateX(int x) =>
                x;

            protected override int IterateY(int y) =>
                ++y;

            protected override (int x, int y) GetInitialCoordinates(Cell cell) =>
                (cell.X, 0);
        }
    }
}
