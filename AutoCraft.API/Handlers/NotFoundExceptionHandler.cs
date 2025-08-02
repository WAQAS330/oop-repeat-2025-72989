using AutoCraft.API.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace AutoCraft.API.Handlers;
// this is the class that will handle the not found exception
public class NotFoundExceptionHandler : IExceptionHandler
{
    public bool CanHandle(Exception exception)
    {
        return exception.Message.Contains("not found") || 
               exception.Message.Contains("not exist") ||
               exception is KeyNotFoundException;
    }

    public async Task<object> HandleExceptionAsync(Exception exception, HttpRequest request)
    {
        return new
        {
            error = "Resource not found",
            message = exception.Message,
            timestamp = DateTime.UtcNow
        };
    }
} 