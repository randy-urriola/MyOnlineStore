using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyOnlineStore.Models;
using MyOnlineStore.Services;

namespace MyOnlineStore.Controllers
{
    public class HomeController(
        CategoryService _categoryService,
        ProductService _productService
        ) : Controller
    {
        

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllAsync();
            var products = await _productService.GetCatalogAsync();
            var catalog = new CatalogVM { Categories = categories, Products = products };
            return View(catalog);
        }

        public async Task<IActionResult> FilterByCategory(int id, string name)
        {
            var categories = await _categoryService.GetAllAsync();
            var products = await _productService.GetCatalogAsync(categoryId:id);

            var catalog = new CatalogVM { Categories = categories, Products = products, filterBy=name};
            return View("Index",catalog);
        }

        [HttpPost]
        public async Task<IActionResult> FilterBySearch(string value)
        {
            var categories = await _categoryService.GetAllAsync();
            var products = await _productService.GetCatalogAsync(search: value);

            var catalog = new CatalogVM { Categories = categories, Products = products, filterBy = $"Results for: {value}" };
            return View("Index", catalog);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
