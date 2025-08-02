using AutoCraft.API.DTOs;
using AutoCraft.API.Handlers;
using AutoCraft.API.Interfaces;
using AutoCraft.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoCraft.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly MyDbContext _context;
    private readonly IEnumerable<IExceptionHandler> _exceptionHandlers;

    public CustomersController(MyDbContext context, IEnumerable<IExceptionHandler> exceptionHandlers)
    {
        _context = context;
        _exceptionHandlers = exceptionHandlers;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerDto>>> GetAllCustomers()
    {
        try
        {
            var customers = await _context.Customers
                .Include(c => c.RegisteredVehicles)
                .Select(c => new CustomerDto
                {
                    CustomerIdentifier = c.CustomerIdentifier,
                    FullName = c.FullName,
                    EmailAddress = c.EmailAddress,
                    RegisteredVehicleIds = c.RegisteredVehicles != null ? c.RegisteredVehicles.Select(v => v.VehicleId).ToList() : new List<int>()
                })
                .ToListAsync();

            return Ok(customers);
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
    public async Task<ActionResult<CustomerDto>> GetCustomerById(int id)
    {
        try
        {
            var customer = await _context.Customers
                .Include(c => c.RegisteredVehicles)
                .FirstOrDefaultAsync(c => c.CustomerIdentifier == id);

            if (customer == null)
            {
                throw new KeyNotFoundException($"Customer with ID {id} not found");
            }

            var customerDto = new CustomerDto
            {
                CustomerIdentifier = customer.CustomerIdentifier,
                FullName = customer.FullName,
                EmailAddress = customer.EmailAddress,
                RegisteredVehicleIds = customer.RegisteredVehicles != null ? customer.RegisteredVehicles.Select(v => v.VehicleId).ToList() : new List<int>()
            };

            return Ok(customerDto);
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