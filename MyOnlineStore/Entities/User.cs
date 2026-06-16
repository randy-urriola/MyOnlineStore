using System.ComponentModel.DataAnnotations;

namespace MyOnlineStore.Entities
{
    // Entidades que representan a las tablas
    public class User
    {
        public int UserId { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Type { get; set; }

        public ICollection<Order> Orders { get; set; } // devolvera todas las ordenes de un usuario
    }
}
