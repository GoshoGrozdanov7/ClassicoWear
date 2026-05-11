using Microsoft.AspNetCore.Mvc;

namespace MVC.Intro.Controllers
{
    public class AccessoriesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
