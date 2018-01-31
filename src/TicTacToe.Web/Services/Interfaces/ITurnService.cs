using System;
using System.Threading.Tasks;

namespace TicTacToe.Web
{
    public interface ITurnService : IDisposable
    {
        Task<TurnInfoViewModel> MakeTurn(CellViewModel cell);
    }
}
