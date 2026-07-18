using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyOnlineStore.Models;
using MyOnlineStore.Services;

namespace MyOnlineStore.Controllers
{
    [Authorize(Roles ="Admin")]
    public class CategoryController(CategoryService _categoryService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllAsync();
            return View(categories);
        }

        [HttpGet]
        public async Task<IActionResult> AddEdit(int id)
        {
            var categoryVM = await _categoryService.GetByIdAsync(id);
            return View(categoryVM);
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(CategoryVM entityVM) // metodo para agregar o editar una categoria
        {
            ViewBag.Message = null;
            if (!ModelState.IsValid) return View(entityVM);

            if(entityVM.CategoryId == 0)
            {
                await _categoryService.AddAsync(entityVM);
                ModelState.Clear();
                entityVM = new CategoryVM();
                ViewBag.Message = "Category added successfully!";
            }
            else
            {
                await _categoryService.EditAsync(entityVM);
                ViewBag.Message = "Category edited successfully!";
            }


                return View(entityVM);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _categoryService.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
