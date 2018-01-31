using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace TicTacToe.Web.Controllers
{
    [Route("api/turns")]
    public class TurnApiController : Controller
    {
        private readonly ITurnService _turnService;

        public TurnApiController(ITurnService turnService)
        {
            _turnService = turnService;
        }

        [HttpPost]
        public async Task<JsonResult> MakeTurn([FromBody]CellViewModel cell) =>
            Json(await _turnService.MakeTurn(cell));

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _turnService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
