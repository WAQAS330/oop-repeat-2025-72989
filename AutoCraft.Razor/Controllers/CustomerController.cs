using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoCraft.Domain;
using AutoCraft.Razor.Models;

namespace AutoCraft.Razor.Controllers
{
    public class CustomerController : BaseController
    {
        private readonly MyDbContext context;

        public CustomerController(MyDbContext context)
        {
            this.context = context;
        }

        private bool IsCustomer()
        {
            return HttpContext.Session.GetString("UserRole") == "Customer";
        }

        private int GetCustomerId()
        {
            string sessionId = HttpContext.Session.GetString("UserSession");
            int.TryParse(sessionId, out int customerId);
            return customerId;
        }

        public async Task<IActionResult> Index()
        {
            if (!IsCustomer())
                return RedirectToAction("Login", "Home");

            int customerId = GetCustomerId();
            if (customerId == 0)
                return RedirectToAction("Login", "Home");

            var customerCars = await context.Cars
                .Where(car => car.VehicleOwnerId == customerId)
                .Include(car => car.MaintenanceServices)
                    .ThenInclude(service => service.Mechanic)
                .ToListAsync();

            var carDTOs = customerCars.Select(car => new CarDTO
            {
                VehicleId = car.VehicleId,
                LicensePlateNumber = car.LicensePlateNumber,
                MaintenanceServices = car.MaintenanceServices.Select(s => new ServiceDTO
                {
                    ServiceId = s.ServiceId,
                    ServiceDate = s.ServiceDate,
                    MechanicName = s.Mechanic != null ? s.Mechanic.FullName : "N/A",
                    WorkDescription = s.WorkDescription,
                    Status = s.Status,
                    Hours = s.Hours,
                    CompletionDate = s.CompletionDate,
                    TotalCost = s.TotalCost
                }).ToList()
            }).ToList();

            return View(carDTOs);
        }
    }
}