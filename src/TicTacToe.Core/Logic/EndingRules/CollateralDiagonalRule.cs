namespace TicTacToe.Core
{
    public partial class Game
    {
        private class CollateralDiagonalRule : WinnerEndingRule
        {
            public CollateralDiagonalRule(Game game) : base(game)
            { }

            protected override bool CheckX(int x) =>
                x < GameConstants.TABLE_SIZE;

            protected override bool CheckY(int y) =>
                y >= 0;

            protected override int IterateX(int x) =>
                ++x;

            protected override int IterateY(int y) =>
                --y;

            protected override (int x, int y) GetInitialCoordinates(Cell cell)
            {
                int initialX = cell.X;
                int initialY = cell.Y;

                int bound = GameConstants.TABLE_SIZE - 1;
                while (initialX > 0 && initialY <= bound)
                {
                    --initialX;
                    ++initialY;
                }

                return (initialX, initialY);
            }
        }
    }
}
