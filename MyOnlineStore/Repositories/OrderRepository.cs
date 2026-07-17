using Microsoft.EntityFrameworkCore;
using MyOnlineStore.Context;
using MyOnlineStore.Entities;

namespace MyOnlineStore.Repositories
{
    public class OrderRepository : GenericRepository<Order>
    {
        private readonly AppDbContext _dbContext;
        public OrderRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        // Metodo para alterar la base de datos cuando se haga la compra de los productos en el carrito
        public override async Task AddAsync(Order order)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                foreach (var detail in order.OrderItems)
                {
                    var product = await _dbContext.Product.FindAsync(detail.ProductId);
                    product.Stock -= detail.Quantity;
                }

                await _dbContext.Order.AddAsync(order);
                await _dbContext.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        // Historial de todas las ordenes de un usuario en especifico
        public async Task<IEnumerable<Order>> GetAllWithDetailAsync(int userId)
        {
            var orders = await _dbContext.Order.Where(x => x.UserId == userId)
                .Include(x => x.OrderItems).ThenInclude(x => x.Product).ToListAsync();
            return orders;
        }
    }
}
