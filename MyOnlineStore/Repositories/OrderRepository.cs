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
                foreach(var detail in order.OrderItems)
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
    }
}
