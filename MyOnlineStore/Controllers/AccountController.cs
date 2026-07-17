using Microsoft.AspNetCore.Mvc;
using MyOnlineStore.Models;
using MyOnlineStore.Services;

namespace MyOnlineStore.Controllers
{
    public class AccountController(UserService _userService) : Controller
    {
        public IActionResult Login()
        {
            var viewModel = new LoginVM();
            return View(viewModel);
        }

        [HttpPost]
        public async  Task<IActionResult> Login(LoginVM viewmodel) 
        {
            
            if (!ModelState.IsValid) return View(viewmodel);
            var found = await _userService.Login(viewmodel);

            if (found.UserId == 0)
            {
                ViewBag.Message = "No matches found.";
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult Register()
        {
            var viewModel = new UserVM();
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserVM viewmodel)
        {

            if (!ModelState.IsValid) return View(viewmodel);

            try
            {
                await _userService.Register(viewmodel);
                ViewBag.Message = "Your account has been registered, please try logging in.";
                ViewBag.Class = "alert-success";
            }
            catch (Exception ex) {
                ViewBag.Message = ex.Message;
                ViewBag.Class = "alert-danger";
            }

            return View();
        }
    }
}
