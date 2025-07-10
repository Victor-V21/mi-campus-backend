using MiCampus.Database;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Ruta absoluta al archivo .mdf dentro de la carpeta "Data"
var basePath = Path.Combine(Directory.GetCurrentDirectory(), "Data");

// Crear la carpeta si no existe
if (!Directory.Exists(basePath))
    Directory.CreateDirectory(basePath);

// Ruta completa al archivo .mdf
var dbPath = Path.Combine(basePath, "CampusDB.mdf");

// Cadena de conexión construida manualmente
var connectionString = $"Server=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={dbPath};Integrated Security=True;Connect Timeout=30;";

// Inyectar el DbContext con la cadena construida
builder.Services.AddDbContext<CampusDbContext>(options =>
    options.UseSqlServer(connectionString));

// Agregar servicios
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// Middleware y endpoints
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
