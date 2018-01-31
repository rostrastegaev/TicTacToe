using System;

namespace TicTacToe.Core
{
    public interface IPlayer : IEquatable<IPlayer>
    {
        PlayerMark Mark { get; }
        void FillCell(Cell cell);
    }
}
