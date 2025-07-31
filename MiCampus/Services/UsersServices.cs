using System.Net;
using System.Net.Mail;
using AutoMapper;
using Mapster;
using MiCampus.Configurations;
using MiCampus.Constants;
using MiCampus.Database;
using MiCampus.Database.Entities;
using MiCampus.Dtos.Common;
using MiCampus.Dtos.Security.Users;
using MiCampus.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace MiCampus.Services
{
    public class UsersServices : IUsersServices
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly RoleManager<RoleEntity> _roleManager;
        private readonly CampusDbContext _context;
        private readonly SmtpSettings _smtpSettings;
        private readonly int PAGE_SIZE;
        private readonly int PAGE_SIZE_LIMIT;


        public UsersServices(
            UserManager<UserEntity> userManager,
            RoleManager<RoleEntity> roleManager,
            CampusDbContext context,
            IConfiguration configuration,
            IOptions<SmtpSettings> smtpSettings
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            PAGE_SIZE = configuration.GetValue<int>("PageSize");
            PAGE_SIZE_LIMIT = configuration.GetValue<int>("PageSizeLimit");
            _smtpSettings = smtpSettings.Value;
        }

        public async Task<ResponseDto<PaginationDto<List<UserDto>>>>
           GetListAsync(string seachTerm = "", int page = 1, int pageSize = 0)
        {
            pageSize = pageSize == 0 ? PAGE_SIZE : pageSize;

            int startIndex = (page - 1) * pageSize;

            IQueryable<UserEntity> usersQuery = _context.Users;

            if (!string.IsNullOrEmpty(seachTerm))
            {
                usersQuery = usersQuery
                    .Where(x => (x.FirstName + " " + x.LastName + " " + x.UserName)
                    .Contains(seachTerm));
            }

            int totalRows = await usersQuery.CountAsync();

            var usersEntity = await usersQuery
                .OrderBy(x => x.FirstName)
                .Skip(startIndex)
                .Take(pageSize)
                .ToListAsync();

            var usersDto = usersEntity.Adapt<List<UserDto>>();

            return new ResponseDto<PaginationDto<List<UserDto>>>
            {
                StatusCode = Constants.HttpStatusCode.OK,
                Status = true,
                Message = "Registros obtenidos correctamente",
                Data = new PaginationDto<List<UserDto>>
                {
                    CurrentPage = page,
                    PageSize = pageSize,
                    TotalItems = totalRows,
                    TotalPages = (int)Math.Ceiling((double)totalRows / pageSize),
                    Items = usersDto,
                    HasNextPage = startIndex + pageSize < PAGE_SIZE_LIMIT &&
                        page < (int)Math.Ceiling((double)totalRows / pageSize),
                    HasPreviousPage = page > 1
                }
            };
        }
        public async Task<ResponseDto<UserDto>> GetOneByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
            {
                return new ResponseDto<UserDto>
                {
                    StatusCode = Constants.HttpStatusCode.NOT_FOUND,
                    Status = false,
                    Message = "Registro no encontrado",
                };
            }

            return new ResponseDto<UserDto>
            {
                StatusCode = Constants.HttpStatusCode.OK,
                Status = true,
                Message = "Registro encontrado",
                Data = user.Adapt<UserDto>()
            };
        }

        public async Task<ResponseDto<UserActionResponseDto>> CreateAsync(UserCreateDto dto)
        {
            if (dto.Roles != null && dto.Roles.Any())
            {
                var existingRoles = await _roleManager
                    .Roles.Select(r => r.Name).ToListAsync();

                var invalidRoles = dto.Roles.Except(existingRoles);

                if (invalidRoles.Any())
                {
                    return new ResponseDto<UserActionResponseDto>
                    {
                        StatusCode = Constants.HttpStatusCode.BAD_REQUEST,
                        Status = false,
                        Message = $"Roles son inválidos: {string.Join(", ", invalidRoles)}"
                    };
                }
            }
            var campusEntity = await _context.Campuses.FirstOrDefaultAsync(c => c.Id == dto.CampusId);

            if (campusEntity is null)
            {
                return new ResponseDto<UserActionResponseDto>
                {
                    StatusCode = Constants.HttpStatusCode.NOT_FOUND,
                    Status = false,
                    Message = "Campus no encontrado"
                };
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var user = dto.Adapt<UserEntity>();

                var createResult = await _userManager.CreateAsync(user, dto.Password);

                if (!createResult.Succeeded)
                {
                    await transaction.RollbackAsync();

                    return new ResponseDto<UserActionResponseDto>
                    {
                        StatusCode = Constants.HttpStatusCode.BAD_REQUEST,
                        Status = false,
                        Message = string.Join(", ", createResult
                            .Errors.Select(e => e.Description))
                    };
                }

                // Asiganar roles al usuario
                if (dto.Roles != null && dto.Roles.Any())
                {
                    var addRolesRusult = await _userManager
                        .AddToRolesAsync(user, dto.Roles);

                    if (!addRolesRusult.Succeeded)
                    {
                        await transaction.RollbackAsync();

                        return new ResponseDto<UserActionResponseDto>
                        {
                            StatusCode = Constants.HttpStatusCode.BAD_REQUEST,
                            Status = false,
                            Message = string.Join(", ", addRolesRusult
                                .Errors.Select(e => e.Description))
                        };
                    }
                }

                // Confirmar transacción
                var responseDto = user.Adapt<UserActionResponseDto>();
                await transaction.CommitAsync();

                return new ResponseDto<UserActionResponseDto>
                {
                    StatusCode = Constants.HttpStatusCode.OK,
                    Status = true,
                    Message = "Registro creado correctamente",
                    Data = responseDto
                };
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();

                return new ResponseDto<UserActionResponseDto>
                {
                    StatusCode = Constants.HttpStatusCode.INTERNAL_SERVER_ERROR,
                    Status = false,
                    Message = "Error interno en el servidor"
                };
            }
        }

        public async Task<ResponseDto<UserActionResponseDto>> EditAsync(
            UserEditDto dto, string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
            {
                return new ResponseDto<UserActionResponseDto>
                {
                    StatusCode = Constants.HttpStatusCode.NOT_FOUND,
                    Status = false,
                    Message = "Registro no encontrado"
                };
            }

            if (dto.Roles != null && dto.Roles.Any())
            {
                var existingRoles = await _roleManager
                    .Roles.Select(r => r.Name).ToListAsync();

                var invalidRoles = dto.Roles.Except(existingRoles);

                if (invalidRoles.Any())
                {
                    return new ResponseDto<UserActionResponseDto>
                    {
                        StatusCode = Constants.HttpStatusCode.BAD_REQUEST,
                        Status = false,
                        Message = $"Roles son inválidos: {string.Join(", ", invalidRoles)}"
                    };
                }
            }

            using var transacction = await _context.Database.BeginTransactionAsync();

            try
            {
                dto.Adapt(user);

                var updateResult = await _userManager.UpdateAsync(user);

                if (!updateResult.Succeeded)
                {
                    await transacction.RollbackAsync();

                    return new ResponseDto<UserActionResponseDto>
                    {
                        StatusCode = Constants.HttpStatusCode.BAD_REQUEST,
                        Status = false,
                        Message = string.Join(", ", updateResult
                        .Errors.Select(e => e.Description))
                    };
                }

                if (dto.Roles is not null)
                {
                    var currentRoles = await _userManager.GetRolesAsync(user);
                    var rolesToAdd = dto.Roles.Except(currentRoles).ToList();
                    var rolesToRemove = currentRoles.Except(dto.Roles).ToList();

                    if (rolesToAdd.Any())
                    {
                        var addResult = await _userManager
                            .AddToRolesAsync(user, rolesToAdd);

                        if (!addResult.Succeeded)
                        {
                            await transacction.RollbackAsync();

                            return new ResponseDto<UserActionResponseDto>
                            {
                                StatusCode = Constants.HttpStatusCode.BAD_REQUEST,
                                Status = false,
                                Message = $"Error al agregar roles: " +
                                $"{string.Join(", ", addResult.Errors.Select(e => e.Description))}"
                            };
                        }
                    }

                    if (rolesToRemove.Any())
                    {
                        var removeResult = await _userManager
                            .RemoveFromRolesAsync(user, rolesToRemove);

                        if (!removeResult.Succeeded)
                        {
                            await transacction.RollbackAsync();

                            return new ResponseDto<UserActionResponseDto>
                            {
                                StatusCode = Constants.HttpStatusCode.BAD_REQUEST,
                                Status = false,
                                Message = $"Error al borrar roles: " +
                                $"{string.Join(", ", removeResult.Errors.Select(e => e.Description))}"
                            };
                        }
                    }
                }

                var response = user.Adapt<UserActionResponseDto>();
                await transacction.CommitAsync();

                return new ResponseDto<UserActionResponseDto>
                {
                    StatusCode = Constants.HttpStatusCode.OK,
                    Status = true,
                    Message = "Registro editado correctamente",
                    Data = response
                };
            }
            catch (Exception)
            {
                await transacction.RollbackAsync();

                return new ResponseDto<UserActionResponseDto>
                {
                    StatusCode = Constants.HttpStatusCode.INTERNAL_SERVER_ERROR,
                    Status = false,
                    Message = "Error interno del servidor"
                };
            }

        }

        public async Task<ResponseDto<UserActionResponseDto>> DeleteAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
            {
                return new ResponseDto<UserActionResponseDto>
                {
                    StatusCode = Constants.HttpStatusCode.NOT_FOUND,
                    Status = false,
                    Message = "Registro no encontrado"
                };
            }

            using var transaction = await _context.Database
                .BeginTransactionAsync();

            try
            {
                var userResponse = user.Adapt<UserActionResponseDto>();

                var currentRoles = await _userManager.GetRolesAsync(user);

                if (currentRoles.Any())
                {
                    var removeRolesResult = await _userManager
                        .RemoveFromRolesAsync(user, currentRoles);

                    if (!removeRolesResult.Succeeded)
                    {
                        await transaction.RollbackAsync();

                        return new ResponseDto<UserActionResponseDto>
                        {
                            StatusCode = Constants.HttpStatusCode.BAD_REQUEST,
                            Status = false,
                            Message = $"Error al remover roles: {string.Join(", ", removeRolesResult
                            .Errors.Select(e => e.Description))}"
                        };
                    }
                }

                var deleteUserResult = await _userManager.DeleteAsync(user);
                if (!deleteUserResult.Succeeded)
                {
                    await transaction.RollbackAsync();

                    return new ResponseDto<UserActionResponseDto>
                    {
                        StatusCode = Constants.HttpStatusCode.BAD_REQUEST,
                        Status = false,
                        Message = string.Join(", ", deleteUserResult
                            .Errors.Select(e => e.Description))
                    };
                }

                await transaction.CommitAsync();

                return new ResponseDto<UserActionResponseDto>
                {
                    StatusCode = Constants.HttpStatusCode.OK,
                    Status = true,
                    Message = "Registro borrado correctamente",
                    Data = userResponse
                };
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();

                return new ResponseDto<UserActionResponseDto>
                {
                    StatusCode = Constants.HttpStatusCode.INTERNAL_SERVER_ERROR,
                    Status = false,
                    Message = "Error interno del servidor"
                };
            }
        }

        public async Task<ResponseDto<UserActionResponseDto>> RegisterAsync(UserCreateDto dto)
        {
            // Validar dominio de correo institucional
            if (!dto.Email.EndsWith("@unah.hn", StringComparison.OrdinalIgnoreCase) &&
                !dto.Email.EndsWith("@unah.hn.edu", StringComparison.OrdinalIgnoreCase))
            {
                return new ResponseDto<UserActionResponseDto>
                {
                    StatusCode = Constants.HttpStatusCode.BAD_REQUEST,
                    Status = false,
                    Message = "Solo se permiten correos institucionales (@unah.hn o @unah.hn.edu)."
                };
            }

            // Verificar campus
            var campusEntity = await _context.Campuses.FirstOrDefaultAsync(c => c.Id == dto.CampusId);
            if (campusEntity is null)
            {
                return new ResponseDto<UserActionResponseDto>
                {
                    StatusCode = Constants.HttpStatusCode.NOT_FOUND,
                    Status = false,
                    Message = "Campus no encontrado."
                };
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // 1. Buscar si ya existe un usuario con ese correo
                var existingUser = await _userManager.FindByEmailAsync(dto.Email);

                if (existingUser != null)
                {
                    // Si el correo ya está confirmado, no se puede registrar de nuevo
                    if (existingUser.EmailConfirmed)
                    {
                        return new ResponseDto<UserActionResponseDto>
                        {
                            StatusCode = Constants.HttpStatusCode.BAD_REQUEST,
                            Status = false,
                            Message = "Ya existe una cuenta con este correo y ya ha sido confirmada."
                        };
                    }

                    // Si no está confirmado, podemos sobrescribir el registro o reenviar el correo.
                    // La opción más simple es sobrescribir los datos y reenviar el correo.
                    // También puedes agregar aquí una lógica para validar si el token anterior ha expirado.
                    // Para simplificar, simplemente eliminamos el usuario existente.
                    var deleteResult = await _userManager.DeleteAsync(existingUser);
                    if (!deleteResult.Succeeded)
                    {
                        // Manejar error si no se puede eliminar el usuario
                        await transaction.RollbackAsync();
                        return new ResponseDto<UserActionResponseDto>
                        {
                            StatusCode = Constants.HttpStatusCode.INTERNAL_SERVER_ERROR,
                            Status = false,
                            Message = "No se pudo eliminar el usuario anterior para un nuevo registro."
                        };
                    }
                }

                // Mapear datos del usuario
                var user = dto.Adapt<UserEntity>();
                user.UserName = dto.Email;
                user.EmailConfirmed = false;

                // Crear usuario
                var createResult = await _userManager.CreateAsync(user, dto.Password);
                if (!createResult.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return new ResponseDto<UserActionResponseDto>
                    {
                        StatusCode = Constants.HttpStatusCode.BAD_REQUEST,
                        Status = false,
                        Message = string.Join(", ", createResult.Errors.Select(e => e.Description))
                    };
                }

                // Recargar el usuario para asegurar que tiene SecurityStamp y Id
                user = await _userManager.FindByEmailAsync(dto.Email);

                // Forzar rol normal_user
                var roles = new List<string> { "normal_user" };
                var addRolesResult = await _userManager.AddToRolesAsync(user, roles);
                if (!addRolesResult.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return new ResponseDto<UserActionResponseDto>
                    {
                        StatusCode = Constants.HttpStatusCode.BAD_REQUEST,
                        Status = false,
                        Message = string.Join(", ", addRolesResult.Errors.Select(e => e.Description))
                    };
                }

                // Generar token de confirmación
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                // Crear enlace de confirmación
                var confirmLink = $"https://localhost:7168/api/users/confirm-email?userId={user.Id}&token={Uri.EscapeDataString(token)}";

                // Enviar correo de verificación
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_smtpSettings.FromEmail, _smtpSettings.FromName),
                    Subject = "Confirma tu correo institucional",
                    Body = $"Bienvenido a Mi Campus.\n\nHemos enviado el siguiente enlace para confirmar tu cuenta:\nHaz clic en este enlace para confirmar tu cuenta:\n{confirmLink}",
                    IsBodyHtml = false,
                };
                mailMessage.To.Add(user.Email);

                using var smtp = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port)
                {
                    Credentials = new NetworkCredential(_smtpSettings.UserName, _smtpSettings.Password),
                    EnableSsl = _smtpSettings.EnableSSL
                };

                await smtp.SendMailAsync(mailMessage);

                await transaction.CommitAsync();

                return new ResponseDto<UserActionResponseDto>
                {
                    StatusCode = Constants.HttpStatusCode.OK,
                    Status = true,
                    Message = "Registro exitoso. Revisa tu bandeja de entrada para confirmar tu cuenta.",
                    Data = user.Adapt<UserActionResponseDto>()
                };
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                return new ResponseDto<UserActionResponseDto>
                {
                    StatusCode = Constants.HttpStatusCode.INTERNAL_SERVER_ERROR,
                    Status = false,
                    Message = "Error interno en el servidor: " + e.Message
                };
            }
        }
    }
}
