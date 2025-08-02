using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using AutoCraft.Domain;
using AutoCraft.Razor.Models;
using ErrorViewModel = AutoCraft.Domain.ErrorViewModel;

namespace AutoCraft.Razor.Controllers
{
    public class HomeController : BaseController
    {
        private readonly MyDbContext context;

        public HomeController(MyDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                return RedirectToAction("Dashboard");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Customer user)
        {
            var customer = await context.Customers
                .FirstOrDefaultAsync(x => x.EmailAddress == user.EmailAddress && x.AccountPassword == user.AccountPassword);

            var mechanic = await context.Mechanics
                .FirstOrDefaultAsync(x => x.EmailAddress == user.EmailAddress && x.AccountPassword == user.AccountPassword);

            var admin = await context.Admins
                .FirstOrDefaultAsync(x => x.EmailAddress == user.EmailAddress && x.AccountPassword == user.AccountPassword);

            if (mechanic != null)
            {
                HttpContext.Session.SetString("UserSession", $"{mechanic.MechanicId}");
                HttpContext.Session.SetString("UserRole", "Mechanic");
                return RedirectToAction("Index", "Mechanic");
            }

            if (admin != null)
            {
                HttpContext.Session.SetString("UserSession", "admin");
                HttpContext.Session.SetString("UserRole", "Admin");
                return RedirectToAction("Index", "Admin");
            }

            if (customer != null)
            {
                HttpContext.Session.SetString("UserSession", $"{customer.CustomerIdentifier}");
                HttpContext.Session.SetString("UserRole", "Customer");
                return RedirectToAction("Index", "Customer");
            }
            else
            {
                ViewBag.Message = "Bad Credential Email or Password.";
            }

            return View();
        }

        public IActionResult TerminateUserSession()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                HttpContext.Session.Remove("UserSession");
                HttpContext.Session.Remove("UserRole");
                return RedirectToAction("Login");
            }
            return RedirectToAction("Login");
        }

        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                ViewData["MySession"] = HttpContext.Session.GetString("UserSession").ToString();
                var userRole = HttpContext.Session.GetString("UserRole");

                return userRole switch
                {
                    "Admin" => RedirectToAction("Index", "Admin"),
                    "Mechanic" => RedirectToAction("Index", "Mechanic"),
                    "Customer" => RedirectToAction("Index", "Customer"),
                    _ => View()
                };
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                HttpContext.Session.Remove("UserSession");
                HttpContext.Session.Remove("UserRole");
                return RedirectToAction("Login");
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}