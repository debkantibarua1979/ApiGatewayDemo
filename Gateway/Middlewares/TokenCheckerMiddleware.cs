namespace Gateway.Middlewares;

public class TokenCheckerMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        string requestPath = context.Request.Path.Value!;
        if (
            requestPath.Contains("account/login", StringComparison.InvariantCultureIgnoreCase) ||
            requestPath.Contains("account/register", StringComparison.InvariantCultureIgnoreCase) ||
            requestPath.Equals("/")
        )
        {
            await next(context);
        }
        else
        {
            var authorizationHeader = context.Request.Headers.Authorization;

            if (authorizationHeader.FirstOrDefault() is null)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Access Denied!");
            }
            else
            {
                await next(context);
            }
        }
    }
}