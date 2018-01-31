using TicTacToe.Core;
using TicTacToe.Entities;

namespace TicTacToe
{
    public static class CoreExtensions
    {
        public static Cell ToCell(this TurnEntity turn) =>
            new Cell(turn.X, turn.Y);

        public static TurnEntity ToEntity(this Cell cell, int gameId = 0) =>
            new TurnEntity { X = cell.X, Y = cell.Y, GameId = gameId };
    }
}
