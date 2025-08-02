using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoCraft.Domain;

namespace AutoCraft.Razor.Controllers
{
    public class CarController : BaseController
    {
        private readonly MyDbContext context;

        public CarController(MyDbContext context)
        {
            this.context = context;
        }

        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("UserRole") == "Admin";
        }

        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCar(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Home");

            var car = context.Cars.Include(c => c.MaintenanceServices)
                                   .FirstOrDefault(c => c.VehicleId == id);

            if (car == null)
            {
                return NotFound();
            }

            if (car.MaintenanceServices.Any())
            {
                context.Services.RemoveRange(car.MaintenanceServices);
            }

            context.Cars.Remove(car);
            context.SaveChanges();

            return Ok();
        }
        [HttpPost]
        public IActionResult AddNewCar(string licensePlateNumber, int vehicleOwnerId)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Home");

            var exists = context.Cars.Any(c => c.LicensePlateNumber == licensePlateNumber && c.VehicleOwnerId == vehicleOwnerId);

            if (exists)
            {
                TempData["CarExists"] = "Car with this license plate number already exists.";
                return RedirectToAction("CustomerDetails", "Admin", new { id = vehicleOwnerId });
            }

            var newCar = new Car
            {
                LicensePlateNumber = licensePlateNumber,
                VehicleOwnerId = vehicleOwnerId
            };

            context.Cars.Add(newCar);
            context.SaveChanges();

            return RedirectToAction("CustomerDetails", "Admin", new { id = vehicleOwnerId });
        }
    }
}