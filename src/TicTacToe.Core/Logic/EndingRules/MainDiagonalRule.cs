namespace TicTacToe.Core
{
    public partial class Game
    {
        private class MainDiagonalRule : WinnerEndingRule
        {
            public MainDiagonalRule(Game game) : base(game)
            { }

            protected override bool CheckX(int x) =>
                x < GameConstants.TABLE_SIZE;

            protected override bool CheckY(int y) =>
                y < GameConstants.TABLE_SIZE;

            protected override int IterateX(int x) =>
                ++x;

            protected override int IterateY(int y) =>
                ++y;

            protected override (int x, int y) GetInitialCoordinates(Cell cell)
            {
                int initialX = cell.X;
                int initialY = cell.Y;

                while (initialX > 0 && initialY > 0)
                {
                    --initialX;
                    --initialY;
                }

                return (initialX, initialY);
            }
        }
    }
}
