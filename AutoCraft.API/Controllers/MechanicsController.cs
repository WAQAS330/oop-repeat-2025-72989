using AutoCraft.API.DTOs;
using AutoCraft.API.Handlers;
using AutoCraft.API.Interfaces;
using AutoCraft.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoCraft.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MechanicsController : ControllerBase
{
    private readonly MyDbContext _context;
    private readonly IEnumerable<IExceptionHandler> _exceptionHandlers;

    public MechanicsController(MyDbContext context, IEnumerable<IExceptionHandler> exceptionHandlers)
    {
        _context = context;
        _exceptionHandlers = exceptionHandlers;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MechanicDto>>> GetAllMechanics()
    {
        try
        {
            var mechanics = await _context.Mechanics
                .Select(m => new MechanicDto
                {
                    MechanicId = m.MechanicId,
                    FullName = m.FullName,
                    EmailAddress = m.EmailAddress
                })
                .ToListAsync();

            return Ok(mechanics);
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
    public async Task<ActionResult<MechanicDto>> GetMechanicById(int id)
    {
        try
        {
            var mechanic = await _context.Mechanics
                .FirstOrDefaultAsync(m => m.MechanicId == id);

            if (mechanic == null)
            {
                throw new KeyNotFoundException($"Mechanic with ID {id} not found");
            }

            var mechanicDto = new MechanicDto
            {
                MechanicId = mechanic.MechanicId,
                FullName = mechanic.FullName,
                EmailAddress = mechanic.EmailAddress
            };

            return Ok(mechanicDto);
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