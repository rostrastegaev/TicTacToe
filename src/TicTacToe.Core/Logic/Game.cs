using System.Collections.Generic;
using System.Linq;

namespace TicTacToe.Core
{
    public partial class Game : IGame
    {
        private readonly IPlayersQueueFactory _playersFactory;
        private readonly IPlayersQueue _players;
        private readonly IGameEndingRule[] _rules;
        private readonly List<TurnInfo> _history;
        private readonly Cell[] _table;

        public IEnumerable<TurnInfo> History => _history;

        public Game(IPlayersQueueFactory players)
        {
            _playersFactory = players;
            _players = players.Create();
            _table = new Cell[GameConstants.TABLE_SIZE * GameConstants.TABLE_SIZE];
            for (int i = 0; i < GameConstants.TABLE_SIZE; ++i)
            {
                for (int j = 0; j < GameConstants.TABLE_SIZE; ++j)
                {
                    _table[i * GameConstants.TABLE_SIZE + j] = new Cell(i, j);
                }
            }

            _rules = new IGameEndingRule[]
            {
                new VerticalLineRule(this),
                new HorizontalLineRule(this),
                new MainDiagonalRule(this),
                new CollateralDiagonalRule(this),
                new DrawRule(this)
            };
            _history = new List<TurnInfo>();
        }

        public IEnumerable<Cell> GetAvailableCells() =>
            _table.Where(c => c.IsEmpty);

        public TurnInfo MakeTurn(Cell cell)
        {
            var findedCell = _table.FirstOrDefault(c => c.IsEmpty && c.HasSameCoordinates(cell));
            if (findedCell == null)
            {
                throw new GameException("Invalid turn");
            }

            var player = _players.Next();
            player.FillCell(findedCell);
            var turnInfo = new TurnInfo
            {
                Cell = findedCell,
                Player = player
            };

            foreach (var rule in _rules)
            {
                IGameState ruleState = rule.Check(findedCell);
                if (ruleState.IsEnded)
                {
                    turnInfo.GameState = ruleState;
                    break;
                }
            }

            if (turnInfo.GameState == null)
            {
                turnInfo.GameState = new NotEndedGameState();
            }

            _history.Add(turnInfo);
            return turnInfo;
        }

        public IGame Clone()
        {
            var game = new Game(_playersFactory);
            foreach (var turn in _history)
            {
                game.MakeTurn(turn.Cell);
            }
            return game;
        }
    }
}
