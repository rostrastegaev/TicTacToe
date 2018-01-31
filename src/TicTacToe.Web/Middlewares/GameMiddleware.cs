using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace TicTacToe.Web
{
    public class GameMiddleware
    {
        public RequestDelegate _next;

        public GameMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ICurrentGame currentGame)
        {
            context.Response.OnStarting(() =>
            {
                string gameIdCookieString = context.Request.Cookies[TicTacToeConstants.GAME_ID_COOKIE];
                int.TryParse(gameIdCookieString, out int gameIdCookie);

                if (gameIdCookie != currentGame.GameId)
                {
                    context.Response.Cookies.Delete(TicTacToeConstants.GAME_ID_COOKIE);
                    context.Response.Cookies.Append(TicTacToeConstants.GAME_ID_COOKIE,
                        currentGame.GameId.ToString(), new CookieOptions { HttpOnly = true });
                }

                return Task.CompletedTask;
            });

            await _next(context);
        }
    }
}
