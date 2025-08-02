using Microsoft.AspNetCore.Http;

namespace AutoCraft.API.Interfaces;

public interface IExceptionHandler
{
    // this is the method that will handle the exception
    Task<object> HandleExceptionAsync(Exception exception, HttpRequest request);
    bool CanHandle(Exception exception);
} 