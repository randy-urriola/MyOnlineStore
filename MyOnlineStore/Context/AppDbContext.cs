using Microsoft.EntityFrameworkCore;
using MyOnlineStore.Entities;

namespace MyOnlineStore.Context
{
    public class AppDbContext: DbContext
    {
        // Constructor en donde las opciones se le pasan al DbContext por medio de base()
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        // modelado de entidades como tablas
        public DbSet<Category> Category { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderItem> OrderItem { get; set; }

        // Reescribir las relaciones de las tablas con Fluent API de EF Core
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>(e =>
            {
                e.HasKey("CategoryId");
                e.Property("CategoryId").ValueGeneratedOnAdd();
                e.HasData(
                    new Category { CategoryId = 1, Name = "Technology"},
                    new Category { CategoryId = 2, Name = "Bedroom"}
                 );
            });

            modelBuilder.Entity<Product>(e =>
            {
                e.HasKey("ProductId");
                e.Property("ProductId").ValueGeneratedOnAdd();
                e.Property("Price").HasColumnType("decimal(10,2)");
                e.HasOne(e => e.Category).WithMany(p => p.Products).HasForeignKey(e => e.CategoryId).OnDelete(DeleteBehavior.Restrict); // onDelete restrict evita que al eliminar una categoria se eliminen tambien los productos asociados
            });

            modelBuilder.Entity<User>(e =>
            {
                e.HasKey("UserId");
                e.Property("UserId").ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Order>(e =>
            {
                e.HasKey("OrderId");
                e.Property("OrderId").ValueGeneratedOnAdd();
                e.Property("TotalAmount").HasColumnType("decimal(10,2)");
                e.HasOne(e => e.User).WithMany(p => p.Orders).HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Restrict); // onDelete restrict evita que al eliminar una categoria se eliminen tambien los productos asociados
            });

            modelBuilder.Entity<OrderItem>(e =>
            {
                e.HasKey("OrderItemId");
                e.Property("OrderItemId").ValueGeneratedOnAdd();
                e.Property("Price").HasColumnType("decimal(10,2)");
                e.HasOne(e => e.Order).WithMany(p => p.OrderItems).HasForeignKey(e => e.OrderId).OnDelete(DeleteBehavior.Restrict); // onDelete restrict evita que al eliminar una categoria se eliminen tambien los productos asociados
                
                e.HasOne(e => e.Product).WithMany().HasForeignKey(e => e.ProductId).OnDelete(DeleteBehavior.Restrict);
            });

            
        }
    }
}
