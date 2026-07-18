using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyOnlineStore.Models;
using MyOnlineStore.Services;

namespace MyOnlineStore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductController(ProductService _productService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllAsync();
            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> AddEdit(int id)
        {
            var productVM = await _productService.GetByIdAsync(id);
            return View(productVM);
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(ProductVM entityVM) // metodo para agregar o editar un producto
        {
            ViewBag.Message = null;
            ModelState.Remove("Categories");                    // se remueve la validación de la propiedad Categories y category.Name, 
            ModelState.Remove("Category.Name");                 // ya que no es necesaria para la validación del modelo
            if (!ModelState.IsValid) return View(entityVM);

            if (entityVM.ProductId == 0)
            {
                await _productService.AddAsync(entityVM);
                ModelState.Clear();
                entityVM = new ProductVM();
                ViewBag.Message = "Product created successfully!";
            }
            else
            {
                await _productService.EditAsync(entityVM);
                ViewBag.Message = "Product edited successfully!";
            }


            return View(entityVM);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
