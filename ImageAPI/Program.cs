using Microsoft.EntityFrameworkCore;
using ImageAPI;
using ImageAPI.Repository;

var builder = WebApplication.CreateBuilder(args);

// CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
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

app.UseCors("AllowAll");

// Initialize database
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ImageDbContext>();
    db.Database.EnsureCreated(); // Initialize the database
}

// Swagger
app.UseSwagger();
app.UseSwaggerUI();


app.UseAuthorization();

app.MapControllers();

app.Run();