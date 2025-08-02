
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoCraft.Domain;
using AutoCraft.Razor.Models;

namespace AutoCraft.Razor.Controllers
{
    public class AdminController : BaseController
    {
        private readonly MyDbContext context;

        public AdminController(MyDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<IActionResult> CustomerDetails(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Home");
            ViewData["MySession"] = HttpContext.Session.GetString("UserSession");
            var cars = await context.Cars
                .Include(c => c.VehicleOwner)
                .Include(c => c.MaintenanceServices)
                .Where(c => c.VehicleOwnerId == id)
                .Select(c => new CarDTO
                {
                    VehicleId = c.VehicleId,
                    LicensePlateNumber = c.LicensePlateNumber,
                    VehicleOwnerId = c.VehicleOwnerId,
                    MaintenanceServices = c.MaintenanceServices.Select(s => new ServiceDTO
                    {
                        ServiceId = s.ServiceId
                    }).ToList()
                })
                .ToListAsync();

            ViewBag.CustomerIdentifier = id;

            var customerName = await context.Customers
                .Where(c => c.CustomerIdentifier == id)
                .Select(e => e.FullName)
                .FirstOrDefaultAsync();

            ViewBag.Name = customerName;

            ViewBag.Mechanics = await context.Mechanics
                .Select(m => new MechanicDTO
                {
                    MechanicId = m.MechanicId,
                    FullName = m.FullName
                })
                .ToListAsync();

            return View(cars);
        }

        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("UserRole") == "Admin";
        }

        

        [HttpPost]
        public async Task<IActionResult> AddCustomer(Customer customer)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Home");

            bool emailExists = await context.Customers
                .AnyAsync(c => c.EmailAddress == customer.EmailAddress);

            if (emailExists)
            {
                ModelState.AddModelError("Email", "This email is already registered");
            }

            if (!ModelState.IsValid)
            {
                var allCustomers = await context.Customers
                    .Select(c => new CustomerDTO
                    {
                                            CustomerIdentifier = c.CustomerIdentifier,
                    FullName = c.FullName,
                    EmailAddress = c.EmailAddress,
                    AccountPassword = c.AccountPassword
                    })
                    .ToListAsync();

                ViewBag.NewCustomer = customer;
                return View("Index", allCustomers);
            }

            context.Customers.Add(customer);
            await context.SaveChangesAsync();
            TempData["Success"] = "Customer added successfully!";

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Home");

            var customer = context.Customers.Find(id);
            if (customer != null)
            {
                context.Customers.Remove(customer);
                context.SaveChanges();
                TempData["Success"] = "Customer deleted successfully.";
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Home");
            ViewData["MySession"] = HttpContext.Session.GetString("UserSession");
            var customers = await context.Customers
                .Select(c => new CustomerDTO
                {
                    CustomerIdentifier = c.CustomerIdentifier,
                    FullName = c.FullName,
                    EmailAddress = c.EmailAddress,
                    AccountPassword = c.AccountPassword,
                }).ToListAsync();

            return View(customers);
        }
    }
}

