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
builder.Services.AddDbContext<CampusDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<UserEntity, RoleEntity>()
    .AddEntityFrameworkStores<CampusDbContext>();


builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

// INFERFACES SERVICES
builder.Services.AddTransient<IUsersServices, UsersServices>();
builder.Services.AddTransient<IRolesService, RolesService>();
//builder.Services.AddIdentityCore<RoleEntity>()
//    .AddRoles<RoleEntity>()
//    .AddEntityFrameworkStores<CampusDbContext>();

// Agregar servicios
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