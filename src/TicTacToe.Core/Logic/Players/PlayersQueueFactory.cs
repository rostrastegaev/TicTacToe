using System.Collections.Generic;

namespace TicTacToe.Core
{
    public class PlayersQueueFactory : IPlayersQueueFactory
    {
        private static readonly IEnumerable<IPlayer> _players = new IPlayer[]
        {
            new Player(PlayerMark.X),
            new Player(PlayerMark.O)
        };

        public IPlayersQueue Create() =>
            new PlayersQueue(_players);
    }
}
