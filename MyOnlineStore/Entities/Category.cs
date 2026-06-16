using System.ComponentModel.DataAnnotations;

namespace MyOnlineStore.Entities
{
    // Entidades que representan a las tablas
    public class Category
    {
        public int CategoryId { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
