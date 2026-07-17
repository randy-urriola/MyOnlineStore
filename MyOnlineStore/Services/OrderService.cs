using MyOnlineStore.Entities;
using MyOnlineStore.Models;
using MyOnlineStore.Repositories;

namespace MyOnlineStore.Services
{
    public class OrderService(OrderRepository _orderRepository)
    {
        public async Task AddAsync(List<CartItemVM> cartItemVM, int userId)
        {
            Order order = new Order()
            {
                OrderDate = DateTime.Now,
                UserId = userId,
                TotalAmount = cartItemVM.Sum(x => x.Price * x.Quantity),
                OrderItems = cartItemVM.Select(x => new OrderItem
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    Price = x.Price
                }).ToList()
            };

            await _orderRepository.AddAsync(order);
        }

        public async Task<List<OrderVM>> GetAllByUserAsync(int userId)
        {
            var orders = await _orderRepository.GetAllWithDetailAsync(userId);

            var ordersVM = orders.Select(x => new OrderVM
            {
                OrderDate = x.OrderDate.ToString("MM/dd/yyyy"),
                TotalAmount = x.TotalAmount.ToString("C2"),
                OrderItems = x.OrderItems.Select(x => new OrderItemVM
                {
                    ProductName = x.Product.Name,
                    Quantity = x.Quantity,
                    Price = x.Price.ToString("C2")
                }).ToList()
            }).ToList();

            return ordersVM;

        }
    }
}
