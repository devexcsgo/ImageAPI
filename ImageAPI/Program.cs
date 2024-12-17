using Microsoft.EntityFrameworkCore;
using ImageAPI;
using ImageAPI.Repository;

var builder = WebApplication.CreateBuilder(args);

// CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowAll",
                      policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Controllers
builder.Services.AddControllers();

// Database context
builder.Services.AddDbContext<ImageDbContext>(options =>
{
    options.UseSqlServer(Secret.ConnectionString);
});

// Registrér ImageRepositoryDB som en service
builder.Services.AddScoped<ImageRepositoryDB>();

var app = builder.Build();

// Initialize database
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ImageDbContext>();
    db.Database.EnsureCreated(); // Initialize the database
}

// Swagger
app.UseSwagger();
app.UseSwaggerUI();

// CORS policy
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();