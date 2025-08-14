using MiCampus.Configurations;
using MiCampus.Database;
using MiCampus.Database.Entities;
using MiCampus.Extensions;
using MiCampus.Filters;
using MiCampus.Helpers;
using MiCampus.Hubs;
using MiCampus.Services;
using MiCampus.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Scalar.AspNetCore;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
    };
});

builder.Services.AddHttpContextAccessor();
//builder.WebHost.UseWebRoot("wwwroot"); ya no es compatible
// Configuración de la duración del token de confirmación de correo
builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
{
    // Establece el tiempo de vida del token desde settings.json
    options.TokenLifespan = TimeSpan.FromMinutes(5);
});

// confirmacion de correos electronicos para registrar usuario
builder.Services.Configure<IdentityOptions>(options =>
{
    options.SignIn.RequireConfirmedEmail = true;
});

builder.Services.Configure<SmtpSettings>(
    builder.Configuration.GetSection("SmtpSettings"));


// Servicios de signalR

builder.Services.AddSignalR();

// db

// Conexión a la base de datos WINDOWS
// builder.Services.AddDbContext<CampusDbContext>(options =>
// options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// si se hace una migracion en linux, se debe usar la siguiente cadena de conexión
// para evitar problemas de compatibilidad con el servidor SQL Server en Linux.
builder.Services.AddDbContext<CampusDbContext>(options =>
   options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultLinuxConnection")));

builder.Services.AddIdentity<UserEntity, RoleEntity>(options =>
{
    options.SignIn.RequireConfirmedEmail = true;
})
.AddEntityFrameworkStores<CampusDbContext>()
.AddDefaultTokenProviders(); // <- Necesario para generar tokens

// usamos Mapster para mapear los objetos
MapsterConfig.RegisterMappings();

// no va tener soporte dentro de poco
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));


//config http clients

builder.Services.AddHttpClient<GeminiServices>();

// INFERFACES SERVICES
builder.Services.AddTransient<IUsersServices, UsersServices>();
builder.Services.AddTransient<IRolesService, RolesService>();
builder.Services.AddTransient<ICampusesServices, CampusesServices>();
builder.Services.AddTransient<IPublicationServices, PublicationServices>();
builder.Services.AddTransient<IPublicationTypeServices, PublicationTypeService>();
builder.Services.AddTransient<INotificationServices, NotificationServices>();
builder.Services.AddTransient<INotificationTypeServices, NotificationTypeServices>();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddScoped<IAuditService, AuditService>();
//builder.Services.AddTransient<ICareersServices, CareersServices>();
builder.Services.AddTransient<IGradesServices, GradesServices>();
builder.Services.AddTransient<ICareersServices, CareersServices>();
builder.Services.AddTransient<ISubjectsServices, SubjectsServices>();
builder.Services.AddTransient<IChatsServices, ChatsServices>();
builder.Services.AddScoped<IGeminiServices, GeminiServices>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOpenApi();

var allowUrls = builder.Configuration.GetSection("AllowURLS").Get<string[]>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.WithOrigins(allowUrls) // usa la lista del appsettings.json
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});


// Cargar el archivo .env
DotNetEnv.Env.Load();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

    //swagger
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapHub<ChatHub>("/chathub"); // Mapea el ChatHub a la URL "/chathub"

app.UseHttpsRedirection();

//Agreacion de los cors
app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles(); // para los servicios de imagen

app.MapControllers();

app.Run();
