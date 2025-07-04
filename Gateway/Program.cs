using Gateway.Middlewares;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// dealing with Ocelot config
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policyBuilder =>
        {
            policyBuilder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
        });
});

// Add services to the container.

var app = builder.Build();

app.UseCors();
app.UseHttpsRedirection();

app.UseMiddleware<TokenCheckerMiddleware>();
// Use the InterceptionMiddleware to handle requests before they reach Ocelot
app.UseMiddleware<InterceptionMiddleware>();
app.UseAuthorization();
await app.UseOcelot();              

app.Run();