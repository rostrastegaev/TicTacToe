namespace TicTacToe.Core
{
    public class TurnInfo
    {
        public IPlayer Player { get; set; }
        public Cell Cell { get; set; }
        public IGameState GameState { get; set; }
    }
}
