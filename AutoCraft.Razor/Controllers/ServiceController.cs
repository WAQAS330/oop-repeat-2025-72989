using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoCraft.Domain;
using AutoCraft.Razor.Models;

namespace AutoCraft.Razor.Controllers
{
    public class ServiceController : BaseController
    {
        private readonly MyDbContext context;

        public ServiceController(MyDbContext context)
        {
            this.context = context;
        }

        private bool IsAdminOrMechanic()
        {
            var role = HttpContext.Session.GetString("UserRole");
            return role == "Admin" || role == "Mechanic";
        }

      

        [HttpGet]
        public IActionResult GetServicesForCar(int vehicleId)
        {
            try
            {
                var services = context.Services
                    .Where(s => s.VehicleId == vehicleId)
                    .Include(s => s.Mechanic)
                    .Select(s => new
                    {
                        ServiceId = s.ServiceId,
                        ServiceDate = s.ServiceDate,
                        MechanicName = s.Mechanic != null ? s.Mechanic.FullName : null,
                        WorkDescription = s.WorkDescription,
                        Hours = s.Hours
                    })
                    .ToList();

                return Json(services);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetServicesForCar: {ex.Message}");
                return StatusCode(500, "Error fetching services");
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (!IsAdminOrMechanic())
                return RedirectToAction("Login", "Home");

            var service = context.Services.FirstOrDefault(s => s.ServiceId == id);
            if (service != null)
            {
                context.Services.Remove(service);
                context.SaveChanges();
                return Ok();
            }
            return NotFound();
        }
        
        [HttpPost]
        public IActionResult AddServiceToCar(Service service)
        {
            if (!IsAdminOrMechanic())
                return RedirectToAction("Login", "Home");

            service.CalculateCost();
            service.ServiceDate = DateTime.SpecifyKind(service.ServiceDate, DateTimeKind.Utc);

            context.Services.Add(service);
            context.SaveChanges();

            var vehicleOwnerId = context.Cars
                .Where(c => c.VehicleId == service.VehicleId)
                .Select(c => c.VehicleOwnerId)
                .FirstOrDefault();

            return RedirectToAction("CustomerDetails", "Admin", new { id = vehicleOwnerId });
        }
    }
}
