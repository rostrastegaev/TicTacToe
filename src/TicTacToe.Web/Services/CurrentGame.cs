using Microsoft.AspNetCore.Http;

namespace TicTacToe.Web.Services
{
    public class CurrentGame : ICurrentGame
    {
        public int GameId { get; set; }

        public CurrentGame(IHttpContextAccessor contextAccessor)
        {
            string gameIdCookie = contextAccessor.HttpContext.Request.Cookies[TicTacToeConstants.GAME_ID_COOKIE];
            if (int.TryParse(gameIdCookie, out int gameId))
            {
                GameId = gameId;
            }
        }
    }
}
