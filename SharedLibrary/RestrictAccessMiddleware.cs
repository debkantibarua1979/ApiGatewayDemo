using Microsoft.AspNetCore.Http;

namespace SharedLibrary;

public class RestrictAccessMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var referrer = context.Request.Headers["Referrer"].FirstOrDefault();

        if (string.IsNullOrEmpty(referrer))
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync("You are not allowed to access this resource.");
            return;
        }
        else
        {
            await next(context); 
        }
        
        
    }
}