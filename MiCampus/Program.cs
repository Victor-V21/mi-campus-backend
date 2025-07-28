using MiCampus.Database;
using MiCampus.Database.Entities;
using MiCampus.Helpers;
using MiCampus.Services;
using MiCampus.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

// db

// Conexión a la base de datos WINDOWS
// builder.Services.AddDbContext<CampusDbContext>(options =>
// options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// si se hace una migracion en linux, se debe usar la siguiente cadena de conexión
// para evitar problemas de compatibilidad con el servidor SQL Server en Linux.

builder.Services.AddDbContext<CampusDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultLinuxConnection")));

builder.Services.AddIdentity<UserEntity, RoleEntity>()
    .AddEntityFrameworkStores<CampusDbContext>();

// usamos Mapster para mapear los objetos
MapsterConfig.RegisterMappings();

// no va tener soporte dentro de poco
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

// INFERFACES SERVICES
builder.Services.AddTransient<IUsersServices, UsersServices>();
builder.Services.AddTransient<IRolesService, RolesService>();
builder.Services.AddTransient<ICampusesServices, CampusesServices>();
builder.Services.AddTransient<IGradesServices, GradesServices>();
//builder.Services.AddTransient<ICareersServices, CareersServices>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

    //swagger
    app.UseSwagger();
    app.UseSwaggerUI(); // Esto te da la interfaz bonita
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
