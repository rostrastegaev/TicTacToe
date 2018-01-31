using System.Collections.Generic;

namespace TicTacToe.Core
{
    public class PlayersQueue : IPlayersQueue
    {
        private Queue<IPlayer> _queue;

        public PlayersQueue(IEnumerable<IPlayer> players)
        {
            _queue = new Queue<IPlayer>(players);
        }

        public IPlayer Next()
        {
            var nextPlayer = _queue.Dequeue();
            _queue.Enqueue(nextPlayer);
            return nextPlayer;
        }
    }
}
