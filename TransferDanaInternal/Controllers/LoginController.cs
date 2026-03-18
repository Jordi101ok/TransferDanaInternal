using Microsoft.AspNetCore.Mvc;
using TransferDanaInternal.Models;

namespace TransferDanaInternal.Controllers
{
    public class LoginController : Controller
    {
        public static AppUser currentUser;

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(AppUser user)
        {
            if (currentUser != null && 
                currentUser.UserName.Equals(user.UserName) && 
                currentUser.Password.Equals(user.Password))
            {
                HttpContext.Session.SetString("User", user.UserName);
                HttpContext.Session.SetString("SourceAccount", "ACC001");
                HttpContext.Session.SetInt32("Balance", 1000000);

                return RedirectToAction("Index", "Transfer");
            }
            else
            {
                ViewBag.Message = "Login failed!";
            }

            return View();
        }
        
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(AppUser user)
        {
            currentUser = user;

            return RedirectToAction("Index");
        }
    }
}
