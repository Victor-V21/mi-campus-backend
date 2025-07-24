using AutoMapper;
using Mapster;
using MiCampus.Constants;
using MiCampus.Database;
using MiCampus.Database.Entities;
using MiCampus.Dtos.Common;
using MiCampus.Dtos.Security.Users;
using MiCampus.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MiCampus.Services
{
    public class UsersServices : IUsersServices
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly RoleManager<RoleEntity> _roleManager;
        private readonly CampusDbContext _context;
        private readonly IMapper _mapper;
        private readonly int PAGE_SIZE;
        private readonly int PAGE_SIZE_LIMIT;

        public UsersServices(
            UserManager<UserEntity> userManager,
            RoleManager<RoleEntity> roleManager,
            IMapper mapper,
            CampusDbContext context,
            IConfiguration configuration
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _context = context;
            PAGE_SIZE = configuration.GetValue<int>("PageSize");
            PAGE_SIZE_LIMIT = configuration.GetValue<int>("PageSizeLimit");
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

            var usersDto = _mapper.Map<List<UserDto>>(usersEntity);

            return new ResponseDto<PaginationDto<List<UserDto>>>
            {
                StatusCode = HttpStatusCode.OK,
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
                    StatusCode = HttpStatusCode.NOT_FOUND,
                    Status = false,
                    Message = "Registro no encontrado",
                };
            }

            return new ResponseDto<UserDto>
            {
                StatusCode = HttpStatusCode.OK,
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
                        StatusCode = HttpStatusCode.BAD_REQUEST,
                        Status = false,
                        Message = $"Roles son inválidos: {string.Join(", ", invalidRoles)}"
                    };
                }
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
                        StatusCode = HttpStatusCode.BAD_REQUEST,
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
                            StatusCode = HttpStatusCode.BAD_REQUEST,
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
                    StatusCode = HttpStatusCode.OK,
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
                    StatusCode = HttpStatusCode.INTERNAL_SERVER_ERROR,
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
                    StatusCode = HttpStatusCode.NOT_FOUND,
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
                        StatusCode = HttpStatusCode.BAD_REQUEST,
                        Status = false,
                        Message = $"Roles son inválidos: {string.Join(", ", invalidRoles)}"
                    };
                }
            }

            using var transacction = await _context.Database.BeginTransactionAsync();

            try
            {
                _mapper.Map<UserEditDto, UserEntity>(dto, user);

                var updateResult = await _userManager.UpdateAsync(user);

                if (!updateResult.Succeeded)
                {
                    await transacction.RollbackAsync();

                    return new ResponseDto<UserActionResponseDto>
                    {
                        StatusCode = HttpStatusCode.BAD_REQUEST,
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
                                StatusCode = HttpStatusCode.BAD_REQUEST,
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
                                StatusCode = HttpStatusCode.BAD_REQUEST,
                                Status = false,
                                Message = $"Error al borrar roles: " +
                                $"{string.Join(", ", removeResult.Errors.Select(e => e.Description))}"
                            };
                        }
                    }
                }

                await transacction.CommitAsync();

                return new ResponseDto<UserActionResponseDto>
                {
                    StatusCode = HttpStatusCode.OK,
                    Status = true,
                    Message = "Registro editado correctamente",
                    Data = _mapper.Map<UserActionResponseDto>(user)
                };
            }
            catch (Exception)
            {
                await transacction.RollbackAsync();

                return new ResponseDto<UserActionResponseDto>
                {
                    StatusCode = HttpStatusCode.INTERNAL_SERVER_ERROR,
                    Status = false,
                    Message = "Error interno del servidor"
                };
            }

        }

        public async Task<ResponseDto<UserActionResponseDto>> DeleteAsync(
            string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
            {
                return new ResponseDto<UserActionResponseDto>
                {
                    StatusCode = HttpStatusCode.NOT_FOUND,
                    Status = false,
                    Message = "Registro no encontrado"
                };
            }

            using var transaction = await _context.Database
                .BeginTransactionAsync();

            try
            {
                var userResponse = _mapper.Map<UserActionResponseDto>(user);

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
                            StatusCode = HttpStatusCode.BAD_REQUEST,
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
                        StatusCode = HttpStatusCode.BAD_REQUEST,
                        Status = false,
                        Message = string.Join(", ", deleteUserResult
                            .Errors.Select(e => e.Description))
                    };
                }

                await transaction.CommitAsync();

                return new ResponseDto<UserActionResponseDto>
                {
                    StatusCode = HttpStatusCode.OK,
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
                    StatusCode = HttpStatusCode.INTERNAL_SERVER_ERROR,
                    Status = false,
                    Message = "Error interno del servidor"
                };
            }
        }
    }
}
