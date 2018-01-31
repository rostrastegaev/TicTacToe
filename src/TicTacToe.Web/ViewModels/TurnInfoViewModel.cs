using TicTacToe.Core;

namespace TicTacToe.Web
{
    public class TurnInfoViewModel
    {
        public CellViewModel Cell { get; set; }
        public GameStateViewModel GameState { get; set; }
        public PlayerMark Mark { get; set; }
    }
}
