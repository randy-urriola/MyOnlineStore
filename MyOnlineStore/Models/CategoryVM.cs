using System.ComponentModel.DataAnnotations;

namespace MyOnlineStore.Models
{
    public class CategoryVM
    {
        // Las vistas acceden a estas propiedades en lugar de acceder directamente a la entidad Category
        public int CategoryId { get; set; }
        public string Name { get; set; }
    }
}
