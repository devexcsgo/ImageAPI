using Microsoft.EntityFrameworkCore;
using ImageAPI;
using ImageAPI.Repository;

var builder = WebApplication.CreateBuilder(args);

// Tilføj CORS policy (Allow All)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registrér controllers og ImageRepositoryDB
builder.Services.AddControllers();
builder.Services.AddScoped<ImageRepositoryDB>();

// Database context
builder.Services.AddDbContext<ImageDbContext>(options =>
{
    options.UseSqlServer(Secret.ConnectionString);
});

var app = builder.Build();

// Initialiser database (migrering hvis nødvendigt)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ImageDbContext>();
    db.Database.Migrate(); // Bruger Migrate i stedet for EnsureCreated til produktion
}

// Middleware-konfiguration

    app.UseSwagger();
    app.UseSwaggerUI();


// Aktiver CORS
app.UseCors("AllowAll");

// Authorization skal altid være før MapControllers
app.UseAuthorization();

// Map controllers
app.MapControllers();

// Kør applikationen
app.Run();
