namespace TicTacToe.Core
{
    public class GameFactory : IGameFactory
    {
        private readonly IPlayersQueueFactory _playersFactory;

        public GameFactory(IPlayersQueueFactory playersFactory)
        {
            _playersFactory = playersFactory;
        }

        public IGame Create() =>
            new Game(_playersFactory);
    }
}
