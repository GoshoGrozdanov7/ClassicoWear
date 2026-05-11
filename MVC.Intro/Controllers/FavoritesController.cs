using Microsoft.AspNetCore.Mvc;
using MVC.Intro;
using MVC.Intro.Services;
using System.Linq;

namespace MVC.Intro.Controllers
{
    [Route("[controller]/[action]")]
    public class FavoritesController : Controller
    {
        private readonly ProductService _productService;

        public FavoritesController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var ids = HttpContext.Session.GetFavoriteProductIds();
            var products = _productService
                .GetAllProducts()
                .Where(p => ids.Contains(p.Id))
                .ToList();

            return View(products);
        }

        [HttpGet("{id}")]
        public IActionResult Toggle(Guid id, string? returnUrl = null)
        {
            HttpContext.Session.ToggleFavoriteProductId(id);

            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost("{id}")]
        public IActionResult TogglePost(Guid id, string? returnUrl = null)
        {
            HttpContext.Session.ToggleFavoriteProductId(id);

            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("{id}")]
        public IActionResult Remove(Guid id, string? returnUrl = null)
        {
            HttpContext.Session.RemoveFavoriteProductId(id);

            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost("{id}")]
        public IActionResult RemovePost(Guid id, string? returnUrl = null)
        {
            HttpContext.Session.RemoveFavoriteProductId(id);

            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Clear()
        {
            HttpContext.Session.ClearFavoriteProductIds();
            return RedirectToAction(nameof(Index));
        }
    }
}
