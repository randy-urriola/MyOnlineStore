using Microsoft.EntityFrameworkCore;
using MyOnlineStore.Entities;
using MyOnlineStore.Models;
using MyOnlineStore.Repositories;

namespace MyOnlineStore.Services
{
    public class CategoryService(GenericRepository<Category> _categoryRepository)
    {
        // los servicios son unicos, cada entidad tiene su propia logica
        public async Task<IEnumerable<CategoryVM>> GetAllAsync() // Trae las categorias y las mapea a la clase CategoryVM para no usar Category directamente en la vista
        {
            var categories = await _categoryRepository.GetAllAsync();

            var categoriesVM = categories.Select(item =>
            new CategoryVM
            {
                CategoryId = item.CategoryId,
                Name = item.Name
            }).ToList();

            return categoriesVM;
        }

        public async Task AddAsync(CategoryVM viewModel) // Agrega una categoria a la BD
        {
            var entity = new Category
            {
                Name = viewModel.Name,
            };
            await _categoryRepository.AddAsync(entity);
        }

        public async Task<CategoryVM?> GetByIdAsync(int id) // Trae una categoria por su id y la mapea a la clase CategoryVM
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            var categoryVM = new CategoryVM();

            if (category != null)
            {
                categoryVM.Name = category.Name;
                categoryVM.CategoryId = category.CategoryId;
            }

            return categoryVM;
        }

        public async Task EditAsync(CategoryVM viewModel) // Edita una categoria en la BD
        {
            var entity = new Category
            {
                CategoryId = viewModel.CategoryId,
                Name = viewModel.Name,
            };
            await _categoryRepository.EditAsync(entity);
        }

        public async Task DeleteAsync(int id) // Elimina una categoria de la BD
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            await _categoryRepository.DeleteAsync(category);
        }
    }
}
