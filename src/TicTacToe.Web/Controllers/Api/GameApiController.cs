using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TicTacToe.DataAccess;

namespace TicTacToe.Web.Controllers
{
    [Route("api/games")]
    public class GameApiController : Controller
    {
        private readonly IGameService _gameService;

        public GameApiController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpGet]
        public async Task<JsonResult> GetGames([FromQuery]Fetch fetch) =>
            Json(await _gameService.GetGames(fetch));

        [HttpPost]
        public async Task<JsonResult> CreateGame([FromBody]GameViewModel model) =>
            Json(await _gameService.Create(model));

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _gameService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
