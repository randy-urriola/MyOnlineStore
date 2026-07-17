using Microsoft.AspNetCore.Mvc;
using MyOnlineStore.Services;
using System.Threading.Tasks;

namespace MyOnlineStore.Controllers
{
    public class UserController(OrderService _orderService) : Controller
    {
        public async Task<IActionResult> MyOrders()
        {
            // TODO: change id
            var userId = 1;
            var ordersvm = await _orderService.GetAllByUserAsync(userId);
            return View(ordersvm);
        }
    }
}
