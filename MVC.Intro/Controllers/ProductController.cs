using Microsoft.AspNetCore.Mvc;
using MVC.Intro.Models;
using MVC.Intro.Services;

namespace MVC.Intro.Controllers
{
    [Route("[controller]/[action]")]
    public class ProductController : Controller
    {
        ProductService _productService;
        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        public IActionResult Index()
        {
            return View(_productService.GetAllProducts());
        }
        [HttpGet("{id}")]//https://localhost:7206/Product/Details/52e9ba06-3b56-4f3d-973c-388efb0e4417
        public IActionResult Details(Guid id)
        {
            return View(_productService.GetProductById(id));
        }

        [HttpGet("{id}")]
        public IActionResult Edit(Guid id)
        {
            return View(_productService.GetProductById(id));
        }

        [HttpPost]
        [RequestSizeLimit(10_000_000)]
        public async Task<IActionResult> EditProduct(Product product, IFormFile? image)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", product);
            }

            var updated = _productService.UpdateProduct(product);

            if (image != null && image.Length > 0)
            {
                var imagesPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "products");
                Directory.CreateDirectory(imagesPath);

                var destinationPath = Path.Combine(imagesPath, $"{updated.Id}.jpg");
                await using (var stream = System.IO.File.Create(destinationPath))
                {
                    await image.CopyToAsync(stream);
                }
            }

            return RedirectToAction(nameof(Details), new { id = updated.Id });
        }

        [HttpPost("{id}")]
        [RequestSizeLimit(10_000_000)]
        public async Task<IActionResult> UploadImage(Guid id, IFormFile image)
        {
            if (image == null || image.Length == 0)
            {
                return RedirectToAction(nameof(Details), new { id });
            }

            var imagesPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "products");
            Directory.CreateDirectory(imagesPath);

            var destinationPath = Path.Combine(imagesPath, $"{id}.jpg");
            await using (var stream = System.IO.File.Create(destinationPath))
            {
                await image.CopyToAsync(stream);
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Delete(Guid id)
        {
            _productService.DeleteProduct(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [RequestSizeLimit(10_000_000)]
        public async Task<IActionResult> CreateProduct(Product product, IFormFile? image)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", product);
            }
            var created = _productService.AddProduct(product);

            if (image != null && image.Length > 0)
            {
                var imagesPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "products");
                Directory.CreateDirectory(imagesPath);

                var destinationPath = Path.Combine(imagesPath, $"{created.Id}.jpg");
                await using (var stream = System.IO.File.Create(destinationPath))
                {
                    await image.CopyToAsync(stream);
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
