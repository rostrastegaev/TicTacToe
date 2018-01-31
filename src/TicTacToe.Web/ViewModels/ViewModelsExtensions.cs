using System.Linq;
using TicTacToe.Core;
using TicTacToe.Entities;

namespace TicTacToe.Web
{
    public static class ViewModelsExtensions
    {
        public static CellViewModel ToViewModel(this Cell cell) =>
            new CellViewModel() { X = cell.X, Y = cell.Y };

        public static CellViewModel ToViewModel(this TurnEntity turn) =>
            new CellViewModel() { X = turn.X, Y = turn.Y };

        public static GameStateViewModel ToViewModel(this IGameState gameState)
        {
            if (gameState is WinnerGameState winner)
            {
                return new WinnerGameStateViewModel()
                {
                    IsEnded = winner.IsEnded,
                    Winner = winner.Winner.Mark,
                    WinningCells = winner.WinningCells.Select(c => c.ToViewModel())
                };
            }

            return new GameStateViewModel()
            {
                IsEnded = gameState.IsEnded
            };
        }

        public static TurnInfoViewModel ToViewModel(this TurnInfo turnInfo) =>
            new TurnInfoViewModel()
            {
                Cell = turnInfo.Cell.ToViewModel(),
                GameState = turnInfo.GameState.ToViewModel(),
                Mark = turnInfo.Player.Mark
            };

        public static GameViewModel ToViewModel(this GameEntity game) =>
            new GameViewModel()
            {
                Id = game.Id,
                IsPlayerFirst = game.IsPlayerFirst,
                StartedAt = game.StartedAt,
                EndedAt = game.EndedAt
            };
    }
}
