using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoCraft.Domain;
using AutoCraft.Razor.Models;

namespace AutoCraft.Razor.Controllers
{
    public class MechanicController : BaseController
    {
        private readonly MyDbContext context;

        public MechanicController(MyDbContext context)
        {
            this.context = context;
        }

        private bool IsMechanic()
        {
            return HttpContext.Session.GetString("UserRole") == "Mechanic";
        }

        private int GetMechanicId()
        {
            string sessionId = HttpContext.Session.GetString("UserSession");
            int.TryParse(sessionId, out int mechanicId);
            return mechanicId;
        }

       
        [HttpPost]
        public async Task<IActionResult> CompleteService(int id, string workDescription, decimal hours)
        {
            if (!IsMechanic())
                return RedirectToAction("Login", "Home");

            var service = context.Services
                .Include(s => s.Vehicle)
                .FirstOrDefault(s => s.ServiceId == id);

            if (service == null)
            {
                return NotFound();
            }

            service.WorkDescription = workDescription;
            service.Hours = hours;
            service.Status = "Completed";
            service.CompletionDate = DateTime.UtcNow;
            service.TotalCost = hours * 75;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Error: {ex.InnerException?.Message}");
                throw;
            }

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Index()
        {
            if (!IsMechanic())
                return RedirectToAction("Login", "Home");

            int mechanicId = GetMechanicId();
            if (mechanicId == 0)
                return RedirectToAction("Login", "Home");

            var services = await context.Services
                .Include(s => s.Vehicle)
                .Include(s => s.Mechanic)
                .Where(s => s.MechanicId == mechanicId)
                .Select(s => new ServiceDTO
                {
                    ServiceId = s.ServiceId,
                    ServiceDate = s.ServiceDate,
                    MechanicName = s.Mechanic != null ? s.Mechanic.FullName : string.Empty,
                    WorkDescription = s.WorkDescription,
                    Hours = s.Hours,
                    VehicleId = s.VehicleId,
                    MechanicId = s.MechanicId,
                    TotalCost = s.TotalCost,
                    LicensePlateNumber = s.Vehicle != null ? s.Vehicle.LicensePlateNumber : string.Empty,
                    Status = s.Status ?? "Pending",
                    CompletionDate = s.CompletionDate
                })
                .ToListAsync();

            return View(services);
        }

    }
}