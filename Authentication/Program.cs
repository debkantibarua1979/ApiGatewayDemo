using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SharedLibrary;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(); // <-- Add this
builder.Services.AddSwaggerGen();  // Optional for Swagger UI
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => 
        {
            var key = Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Authentication:Key").Value!);
            string issuer = builder.Configuration["Authentication:Issuer"]!;
            string audience = builder.Configuration["Authentication:Audience"]!;
             
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            
            options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
            
        }
    );

builder.Services.AddCors(builder =>
{
    builder.AddDefaultPolicy(
        policyBuilder =>
        {
            policyBuilder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
        });
});
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseMiddleware<RestrictAccessMiddleware>();
app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();
app.MapControllers();

app.Run();