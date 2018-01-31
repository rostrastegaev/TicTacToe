using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace TicTacToe.Web.Controllers
{
    [Route("")]
    public class GameController : Controller
    {
        private readonly IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [Route("")]
        public async Task<IActionResult> Index()
        {
            var game = await _gameService.GetCurrent();
            if (game == null)
            {
                game = new GameViewModel();
            }

            return View(game);
        }

        [Route("{id:int}")]
        public async Task<IActionResult> History(int id)
        {
            var game = await _gameService.GetHistory(id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

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
