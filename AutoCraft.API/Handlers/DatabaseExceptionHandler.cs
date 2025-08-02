using AutoCraft.API.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace AutoCraft.API.Handlers;
// this is the class that will handle the database exception
public class DatabaseExceptionHandler : IExceptionHandler
{
    public bool CanHandle(Exception exception)
    {
        return exception is DbUpdateException || 
               exception is InvalidOperationException && exception.Message.Contains("database");
    }

    public async Task<object> HandleExceptionAsync(Exception exception, HttpRequest request)
    {
        return new
        {
            error = "Database operation failed",
            message = "An error occurred while accessing the database",
            timestamp = DateTime.UtcNow
        };
    }
} 