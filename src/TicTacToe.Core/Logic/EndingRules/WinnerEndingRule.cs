using System.Collections.Generic;

namespace TicTacToe.Core
{
    public partial class Game
    {
        private abstract class WinnerEndingRule : IGameEndingRule
        {
            protected readonly Game _game;

            protected WinnerEndingRule(Game game)
            {
                _game = game;
            }

            protected abstract (int x, int y) GetInitialCoordinates(Cell cell);
            protected abstract bool CheckY(int y);
            protected abstract bool CheckX(int x);
            protected abstract int IterateY(int y);
            protected abstract int IterateX(int x);

            public IGameState Check(Cell cell)
            {
                (int x, int y) = GetInitialCoordinates(cell);

                List<Cell> cellsInRow = new List<Cell>(GameConstants.IN_ROW_FOR_WIN);
                IPlayer currentPlayer = null;

                while (CheckX(x) && CheckY(y))
                {
                    Cell currentCell = _game._table[x * GameConstants.TABLE_SIZE + y];
                    if (currentCell.IsEmpty)
                    {
                        cellsInRow.Clear();
                        x = IterateX(x);
                        y = IterateY(y);
                        continue;
                    }

                    if (currentPlayer != currentCell.Owner)
                    {
                        currentPlayer = currentCell.Owner;
                        cellsInRow.Clear();
                    }

                    cellsInRow.Add(currentCell);
                    if (cellsInRow.Count == GameConstants.IN_ROW_FOR_WIN)
                    {
                        return new WinnerGameState(currentPlayer, cellsInRow);
                    }

                    x = IterateX(x);
                    y = IterateY(y);
                }

                return new NotEndedGameState();
            }
        }
    }
}
