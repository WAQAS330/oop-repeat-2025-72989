using AutoCraft.API.DTOs;
using AutoCraft.API.Handlers;
using AutoCraft.API.Interfaces;
using AutoCraft.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoCraft.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarsController : ControllerBase
{
    private readonly MyDbContext _context;
    private readonly IEnumerable<IExceptionHandler> _exceptionHandlers;

    public CarsController(MyDbContext context, IEnumerable<IExceptionHandler> exceptionHandlers)
    {
        _context = context;
        _exceptionHandlers = exceptionHandlers;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CarDto>>> GetAllCars()
    {
        try
        {
            var cars = await _context.Cars
                .Include(c => c.VehicleOwner)
                .Select(c => new CarDto
                {
                    VehicleId = c.VehicleId,
                    LicensePlateNumber = c.LicensePlateNumber,
                    VehicleOwnerId = c.VehicleOwnerId,
                    OwnerName = c.VehicleOwner != null ? c.VehicleOwner.FullName : string.Empty,
                    OwnerEmail = c.VehicleOwner != null ? c.VehicleOwner.EmailAddress : string.Empty
                })
                .ToListAsync();

            return Ok(cars);
        }
        catch (Exception ex)
        {
            var handler = _exceptionHandlers.FirstOrDefault(h => h.CanHandle(ex));
            if (handler != null)
            {
                var errorResponse = await handler.HandleExceptionAsync(ex, Request);
                return StatusCode(500, errorResponse);
            }
            return StatusCode(500, "An unexpected error occurred");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CarDto>> GetCarById(int id)
    {
        try
        {
            var car = await _context.Cars
                .Include(c => c.VehicleOwner)
                .FirstOrDefaultAsync(c => c.VehicleId == id);

            if (car == null)
            {
                throw new KeyNotFoundException($"Car with ID {id} not found");
            }

            var carDto = new CarDto
            {
                VehicleId = car.VehicleId,
                LicensePlateNumber = car.LicensePlateNumber,
                VehicleOwnerId = car.VehicleOwnerId,
                OwnerName = car.VehicleOwner != null ? car.VehicleOwner.FullName : string.Empty,
                OwnerEmail = car.VehicleOwner != null ? car.VehicleOwner.EmailAddress : string.Empty
            };

            return Ok(carDto);
        }
        catch (Exception ex)
        {
            var handler = _exceptionHandlers.FirstOrDefault(h => h.CanHandle(ex));
            if (handler != null)
            {
                var errorResponse = await handler.HandleExceptionAsync(ex, Request);
                if (ex is KeyNotFoundException)
                {
                    return NotFound(errorResponse);
                }
                return StatusCode(500, errorResponse);
            }
            return StatusCode(500, "An unexpected error occurred");
        }
    }
} 