using Microsoft.AspNetCore.Mvc;
using MVC.Intro;
using MVC.Intro.Services;
using System.Linq;

namespace MVC.Intro.Controllers
{
    [Route("[controller]/[action]")]
    public class CartController : Controller
    {
        private readonly ProductService _productService;

        public CartController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var cartItems = HttpContext.Session.GetCartItems();
            var products = _productService.GetAllProducts();

            var lines = products
                .Where(p => cartItems.ContainsKey(p.Id))
                .Select(p => new CartLine
                {
                    Product = p,
                    Quantity = cartItems[p.Id]
                })
                .ToList();

            var model = new CartViewModel
            {
                Lines = lines
            };

            return View(model);
        }

        [HttpGet("{id}")]
        public IActionResult Add(Guid id, string? returnUrl = null)
        {
            HttpContext.Session.AddToCart(id, 1);

            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("{id}")]
        public IActionResult Decrease(Guid id, string? returnUrl = null)
        {
            HttpContext.Session.DecreaseFromCart(id, 1);

            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("{id}")]
        public IActionResult Remove(Guid id, string? returnUrl = null)
        {
            HttpContext.Session.RemoveFromCart(id);

            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Clear()
        {
            HttpContext.Session.ClearCart();
            return RedirectToAction(nameof(Index));
        }

        public class CartViewModel
        {
            public List<CartLine> Lines { get; set; } = new();

            public decimal Total => Lines.Sum(l => l.Product.Price * l.Quantity);

            public int TotalQuantity => Lines.Sum(l => l.Quantity);
        }

        public class CartLine
        {
            public required Models.Product Product { get; set; }

            public int Quantity { get; set; }

            public decimal LineTotal => Product.Price * Quantity;
        }
    }
}
