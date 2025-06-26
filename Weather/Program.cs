var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(); // <-- Add this
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();  // Optional for Swagger UI

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Add this to map controller routes
app.MapControllers();

app.Run();