using Microsoft.AspNetCore.Mvc;

namespace TicTacToe.Web.Controllers
{
    [Route("error")]
    public class ErrorController : Controller
    {
        [Route("{code:int}")]
        public IActionResult Index(int code)
        {
            return View();
        }

        [Route("404")]
        public IActionResult NotFoundPage()
        {
            return View("NotFound");
        }
    }
}
