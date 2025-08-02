using AutoCraft.API.Interfaces;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace AutoCraft.API.Handlers;
// this is the class that will handle the validation exception
public class ValidationExceptionHandler : IExceptionHandler
{
    public bool CanHandle(Exception exception)
    {
        return exception is ValidationException || 
               exception is ArgumentException;
    }

    public async Task<object> HandleExceptionAsync(Exception exception, HttpRequest request)
    {
        return new
        {
            error = "Validation failed",
            message = exception.Message,
            timestamp = DateTime.UtcNow
        };
    }
} 