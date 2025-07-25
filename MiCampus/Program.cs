using MiCampus.Database;
using MiCampus.Database.Entities;
using MiCampus.Helpers;
using MiCampus.Services;
using MiCampus.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// db

// Conexión a la base de datos WINDOWS
builder.Services.AddDbContext<CampusDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// si se hace una migracion en linux, se debe usar la siguiente cadena de conexión
// para evitar problemas de compatibilidad con el servidor SQL Server en Linux.
/*
builder.Services.AddDbContext<CampusDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultLinuxConnection")));
*/

builder.Services.AddIdentity<UserEntity, RoleEntity>()
    .AddEntityFrameworkStores<CampusDbContext>();


builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

// INFERFACES SERVICES
builder.Services.AddTransient<IUsersServices, UsersServices>();
builder.Services.AddTransient<IRolesService, RolesService>();
//builder.Services.AddTransient<ICampusesServices, CapusesServices>();
//builder.Services.AddTransient<ICareersServices, CareersServices>();

builder.Services.AddControllers();


builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();