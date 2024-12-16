using Microsoft.EntityFrameworkCore;
using ImageAPI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowAll",
                      policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();
builder.Services.AddDbContext<ImageDbContext>(options =>
{
    options.UseSqlServer(Secret.ConnectionString);
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ImageDbContext>();
    db.Database.EnsureCreated(); // Initialize the database
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
