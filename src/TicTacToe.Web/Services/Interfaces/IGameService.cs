using System;
using System.Threading.Tasks;
using TicTacToe.DataAccess;

namespace TicTacToe.Web
{
    public interface IGameService : IDisposable
    {
        Task<GameViewModel> GetCurrent();
        Task<GameViewModel> GetHistory(int gameId);
        Task<FetchResult<GameViewModel>> GetGames(Fetch fetch);
        Task<GameViewModel> Create(GameViewModel game);
    }
}
