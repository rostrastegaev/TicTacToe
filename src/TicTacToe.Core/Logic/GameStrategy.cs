using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicTacToe.Core
{
    public class GameStrategy : IGameStrategy
    {
        private readonly GameTurnNode _head;
        private readonly IGameFactory _gameFactory;

        public GameStrategy(IGameFactory gameFactory)
        {
            _head = new GameTurnNode(null);
            _gameFactory = gameFactory;
        }

        public void Calculate()
        {
            IGame game = _gameFactory.Create();
            var tasks = new List<Task>(GameConstants.TABLE_SIZE * GameConstants.TABLE_SIZE);
            foreach (var cell in game.GetAvailableCells())
            {
                var tempGame = game.Clone();
                GameTurnNode child = _head.AddChild(tempGame.MakeTurn(cell));
                tasks.Add(Task.Run(() => CalculateForNode(child, tempGame)));
            }

            Task.WaitAll(tasks.ToArray());
        }

        public Cell GetBestCell(IEnumerable<TurnInfo> history)
        {
            GameTurnNode findedNode;
            if (history?.Any() ?? false)
            {
                var cellsHistory = history.Select(h => h.Cell);
                findedNode = FindNode(cellsHistory);
            }
            else
            {
                findedNode = _head;
            }

            var bestChild = findedNode.GetBestChild();
            return bestChild.TurnInfo.Cell;
        }

        private void CalculateForNode(GameTurnNode node, IGame game)
        {
            foreach (var cell in game.GetAvailableCells())
            {
                var tempGame = game.Clone();
                var child = node.AddChild(tempGame.MakeTurn(cell));
                if (child.TurnInfo.GameState.IsEnded)
                {
                    return;
                }

                CalculateForNode(child, tempGame);
            }
        }

        private GameTurnNode FindNode(IEnumerable<Cell> history)
        {
            GameTurnNode current = _head;
            foreach (var cell in history)
            {
                current = current.Childs?.FirstOrDefault(n => n.TurnInfo.Cell.HasSameCoordinates(cell));
            }

            return current;
        }

        private class GameTurnNode
        {
            private List<GameTurnNode> _childs;

            public int Level { get; private set; }
            public IEnumerable<GameTurnNode> Childs => _childs;
            public TurnInfo TurnInfo { get; private set; }
            public GameTurnNode Parent { get; private set; }
            public bool IsLeaf => _childs.Count == 0;

            public GameTurnNode(TurnInfo turnInfo)
            {
                TurnInfo = turnInfo;
                _childs = new List<GameTurnNode>();
            }

            public GameTurnNode AddChild(TurnInfo turnInfo)
            {
                var child = new GameTurnNode(turnInfo)
                {
                    Level = Level + 1,
                    Parent = this
                };
                _childs.Add(child);
                return child;
            }

            public GameTurnNode GetBestChild()
            {
                var opponent = TurnInfo?.Player ?? _childs[0]._childs[0].TurnInfo.Player;
                int bestPoints = -GameConstants.STRATEGY_MAX_POINTS * 2;
                GameTurnNode bestChild = null;

                foreach (var child in Childs)
                {
                    var childPoints = child.GetPoints(opponent, Level);
                    if (bestPoints < childPoints)
                    {
                        bestPoints = childPoints;
                        bestChild = child;
                    }
                }

                return bestChild;
            }

            private int GetPoints(IPlayer opponent, int rootLevel)
            {
                if (IsLeaf)
                {
                    if (TurnInfo.GameState is WinnerGameState state)
                    {
                        return state.Winner.Equals(opponent) ?
                            (Level - rootLevel) - GameConstants.STRATEGY_MAX_POINTS :
                            GameConstants.STRATEGY_MAX_POINTS - (Level - rootLevel);
                    }
                    return 0;
                }

                int bestPoints = 0;

                if (!TurnInfo.Player.Equals(opponent))
                {
                    bestPoints = GameConstants.STRATEGY_MAX_POINTS * 2;
                    foreach (var child in Childs)
                    {
                        int childPoints = child.GetPoints(opponent, rootLevel);
                        if (bestPoints > childPoints)
                        {
                            bestPoints = childPoints;
                        }
                    }
                }
                else
                {
                    bestPoints = -GameConstants.STRATEGY_MAX_POINTS * 2;
                    foreach (var child in Childs)
                    {
                        int childPoints = child.GetPoints(opponent, rootLevel);
                        if (bestPoints < childPoints)
                        {
                            bestPoints = childPoints;
                        }
                    }
                }
                return bestPoints;
            }
        }
    }
}
