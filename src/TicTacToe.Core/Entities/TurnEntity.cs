namespace TicTacToe.Entities
{
    public class TurnEntity
    {
        public int Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public int GameId { get; set; }
        public GameEntity Game { get; set; }
    }
}
