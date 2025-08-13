using DotNetEnv;
using Mapster;
using MiCampus.Constants;
using MiCampus.Database;
using MiCampus.Database.Entities;
using MiCampus.Dtos.Common;
using MiCampus.Dtos.Feedback;
using MiCampus.Dtos.Publication;
using MiCampus.Dtos.PublicationImage;
using MiCampus.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MiCampus.Services
{
    public class PublicationServices : IPublicationServices
    {
        private readonly CampusDbContext _context;
        private readonly int PAGE_SIZE;
        private readonly int PAGE_SIZE_LIMIT;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<PublicationServices> _logger;

        public PublicationServices(
            CampusDbContext context,
            IConfiguration configuration,
            IWebHostEnvironment env,
            ILogger<PublicationServices> logger)
        {
            _context = context;
            PAGE_SIZE = configuration.GetValue<int>("PageSize");
            PAGE_SIZE_LIMIT = configuration.GetValue<int>("PageSizeLimit");
            _env = env;
            _logger = logger;
        }
        public async Task<ResponseDto<PaginationDto<List<PublicationDto>>>> GetEnabledListAsync(
         string searchTerm = "", int page = 1, int pageSize = 0)
        {
            pageSize = pageSize == 0 ? PAGE_SIZE : pageSize;
            int startIndex = (page - 1) * pageSize;

            
            IQueryable<PublicationEntity> query = _context.Publications
                .Where(p => p.IsEnabled)
                .Include(p => p.Type)
                .Include(p => p.User)
                .Include(p => p.Images); // Incluir imágenes para que se pueda ver el id 

            // Búsqueda por título o texto
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(p => (p.Title + " " + p.Text).Contains(searchTerm));
            }

            var entities = await query
                .OrderByDescending(p => p.DateCreate)
                .Skip(startIndex)
                .Take(pageSize)
                .Select(p => new PublicationDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Text = p.Text,
                    TypeId = p.TypeId,
                    TypeName = p.Type.Name,
                    UserId = p.UserId,
                    UserName = p.User.FirstName + " " + p.User.LastName,  
                    Images = p.Images.Select(i => i.Adapt<PublicationImageDto>()).ToList(),  // Mapear a PublicationImageDto y la de comentarios
                    Feedbacks = p.Feedbacks.Select(f => f.Adapt<FeedbackDto>()).ToList()
                })
                .ToListAsync();

            int totalRows = await query.CountAsync();

            return new ResponseDto<PaginationDto<List<PublicationDto>>>
            {
                StatusCode = HttpStatusCode.OK,
                Status = true,
                Message = "Registros encontrados correctamente",
                Data = new PaginationDto<List<PublicationDto>>
                {
                    CurrentPage = page,
                    PageSize = pageSize,
                    TotalItems = totalRows,
                    TotalPages = (int)Math.Ceiling((double)totalRows / pageSize),
                    Items = entities,
                    HasNextPage = startIndex + pageSize < PAGE_SIZE_LIMIT &&
                                  page < (int)Math.Ceiling((double)totalRows / pageSize),
                    HasPreviousPage = page > 1
                }
            };
        }



        public async Task<ResponseDto<PublicationDto>> CreateAsync(PublicationCreateDto dto)
        {
            try
            {
                
                var userExists = await _context.Users.AnyAsync(u => u.Id == dto.UserId);
                if (!userExists)
                {
                    return new ResponseDto<PublicationDto>
                    {
                        StatusCode = HttpStatusCode.BAD_REQUEST,
                        Status = false,
                        Message = "El usuario especificado no existe"
                    };
                }


                var typeExists = await _context.PublicationsTypes.AnyAsync(t => t.Id == dto.TypeId);
                if (!typeExists)
                {
                    return new ResponseDto<PublicationDto>
                    {
                        StatusCode = HttpStatusCode.BAD_REQUEST,
                        Status = false,
                        Message = "El tipo de publicación no existe"
                    };
                }

                
                var entity = dto.Adapt<PublicationEntity>(); 
                entity.DateCreate = DateTime.UtcNow;
                entity.DateModify = DateTime.UtcNow;
                entity.IsEnabled = true;

                _context.Publications.Add(entity);
                await _context.SaveChangesAsync();

                
                var publicationDto = entity.Adapt<PublicationDto>(); 

                return new ResponseDto<PublicationDto>
                {
                    StatusCode = HttpStatusCode.CREATED,
                    Status = true,
                    Message = "Publicación creada correctamente",
                    Data = publicationDto
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex); // Log error por cualquier error que se pueda generar se dira en consola 
                return new ResponseDto<PublicationDto>
                {
                    StatusCode = HttpStatusCode.INTERNAL_SERVER_ERROR,
                    Status = false,
                    Message = "Error interno al crear la publicación"
                };
            }
        }
    
        public async Task<ResponseDto<PublicationDto>> GetByIdAsync(string id)
        {
            var entity = await _context.Publications
                .Where(p => p.Id == id && p.IsEnabled)
                .Include(p => p.Type)  
                .Include(p => p.User)  
                .Include(p => p.Images)  
                .Include(p => p.Feedbacks) 
                .FirstOrDefaultAsync();

            if (entity == null)
            {
                return new ResponseDto<PublicationDto>
                {
                    StatusCode = HttpStatusCode.NOT_FOUND,
                    Status = false,
                    Message = "Publicación no encontrada"
                };
            }

            
            var publicationDto = entity.Adapt<PublicationDto>();

            return new ResponseDto<PublicationDto>
            {
                StatusCode = HttpStatusCode.OK,
                Status = true,
                Message = "Publicación encontrada",
                Data = publicationDto
            };
        }

        public async Task<ResponseDto<PublicationDto>> EditAsync(string id, PublicationEditDto dto)
        {
            var entity = await _context.Publications
                .FirstOrDefaultAsync(p => p.Id == id && p.IsEnabled);

            if (entity == null)
            {
                return new ResponseDto<PublicationDto>
                {
                    StatusCode = HttpStatusCode.NOT_FOUND,
                    Status = false,
                    Message = "Publicación no encontrada"
                };
            }

            //Aqui lo que hacemos es el mapeo de los cambios 
            dto.Adapt(entity);
            entity.DateModify = DateTime.UtcNow;

            _context.Publications.Update(entity);
            await _context.SaveChangesAsync();

            
            var publicationDto = entity.Adapt<PublicationDto>();

            return new ResponseDto<PublicationDto>
            {
                StatusCode = HttpStatusCode.OK,
                Status = true,
                Message = "Publicación editada correctamente",
                Data = publicationDto
            };
        }

        public async Task<ResponseDto<object>> DeleteAsync(string id)
        {
            var entity = await _context.Publications.FirstOrDefaultAsync(p => p.Id == id && p.IsEnabled);

            if (entity == null)
            {
                return new ResponseDto<object>
                {
                    StatusCode = HttpStatusCode.NOT_FOUND,
                    Status = false,
                    Message = "Publicación no encontrada"
                };
            }

            entity.IsEnabled = false;
            entity.DateModify = DateTime.UtcNow;

            _context.Publications.Update(entity);
            await _context.SaveChangesAsync();

            return new ResponseDto<object>
            {
                StatusCode = HttpStatusCode.OK,
                Status = true,
                Message = "Publicación eliminada correctamente"
            };
        }

        public async Task<ResponseDto<object>> UploadImageAsync(string publicationId, IFormFile file)
        {
            try
            {
                // Verificar existencia de la publicación
                var pubExists = await _context.Publications.AnyAsync(p => p.Id == publicationId && p.IsEnabled);
                if (!pubExists)
                {
                    return new ResponseDto<object>
                    {
                        StatusCode = HttpStatusCode.NOT_FOUND,
                        Status = false,
                        Message = "Publicación no existe"
                    };
                }

                // Validar archivo
                if (file == null || file.Length == 0)
                {
                    return new ResponseDto<object>
                    {
                        StatusCode = HttpStatusCode.BAD_REQUEST,
                        Status = false,
                        Message = "Archivo vacío o inválido"
                    };
                }


                // Verificar WebRootPath y crear la carpeta 'images/publications' si no existe
                if (string.IsNullOrEmpty(_env.WebRootPath))
                {
                    throw new InvalidOperationException("WebRootPath no configurado");
                }

                var folderPath = Path.Combine(_env.WebRootPath, "images", "publications");

                // Crear la carpeta si no existe
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                    Console.WriteLine($"Directorio creado: {folderPath}"); // Log para verificar la creación
                }

                // Nombre de archivo único usando GUID
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var filePath = Path.Combine(folderPath, fileName);

                // Guardar archivo físicamente en la carpeta que hemos hecho manualmente
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Log para verificar WebRootPath esta bien configurado error antes de colocarlo 
                Console.WriteLine($"WebRootPath: {_env.WebRootPath}");

                if (string.IsNullOrEmpty(_env.WebRootPath))
                {
                    throw new InvalidOperationException("WebRootPath no configurado");
                }


                // Guardar el archivo físicamente en el servidor
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Guardar la URL relativa de la imagen en la base de datos
                var imageUrl = $"/images/publications/{fileName}";
                var imageEntity = new PublicationImageEntity
                {
                    PublicationId = publicationId,
                    FileName = file.FileName,
                    Url = imageUrl,
                    DateUpload = DateTime.UtcNow,
                    IsEnabled = true
                };

                _context.PublicationsImages.Add(imageEntity);
                await _context.SaveChangesAsync();

                
                var imageDto = new
                {
                    imageUrl = imageUrl,
                    fileName = file.FileName
                };

                return new ResponseDto<object>
                {
                    StatusCode = HttpStatusCode.CREATED,
                    Status = true,
                    Message = "Imagen subida correctamente",
                    Data = imageDto
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al subir imagen: " + ex.Message);
                return new ResponseDto<object>
                {
                    StatusCode = HttpStatusCode.INTERNAL_SERVER_ERROR,
                    Status = false,
                    Message = "Error al subir la imagen"
                };
            }
        }

        public async Task<ResponseDto<object>> DeleteImageAsync(string publicationId, string imageId)
        {
            try
            {
                var image = await _context.PublicationsImages
                    .FirstOrDefaultAsync(i => i.Id == imageId && i.PublicationId == publicationId && i.IsEnabled);
                if (image == null)
                {
                    return new ResponseDto<object>
                    {
                        StatusCode = HttpStatusCode.NOT_FOUND,
                        Status = false,
                        Message = "Imagen no encontrada",
                        Data = null
                    };
                }

                // Eliminar archivo físico
                var filePath = Path.Combine(_env.WebRootPath, image.Url.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                // Marcar imagen como deshabilitada en la base de datos
                image.IsEnabled = false;
                _context.PublicationsImages.Update(image);
                await _context.SaveChangesAsync();

                // Devolver la información de la imagen eliminada en el campo 'data' para no tener error de eliminacion 
                var imageDto = new
                {
                    Id = image.Id,
                    FileName = image.FileName,
                    Url = image.Url
                };
                return new ResponseDto<object>
                {
                    StatusCode = HttpStatusCode.OK,
                    Status = true,
                    Message = "Imagen eliminada correctamente",
                    Data = imageDto
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar imagen: {ex.Message}");
                return new ResponseDto<object>
                {
                    StatusCode = HttpStatusCode.INTERNAL_SERVER_ERROR,
                    Status = false,
                    Message = "Error interno al eliminar la imagen"
                };
            }
        }
        // Aqui hacemos los servicios para que el usuario haga los comentarios de las publicaciones ya creadas 
        public async Task<ResponseDto<FeedbackDto>> CreateFeedbackAsync(string publicationId, FeekbackCreateDto dto)
        {
            try
            {
                
                var publicationExists = await _context.Publications.AnyAsync(p => p.Id == publicationId && p.IsEnabled);
                if (!publicationExists)
                {
                    return new ResponseDto<FeedbackDto>
                    {
                        StatusCode = HttpStatusCode.NOT_FOUND,
                        Status = false,
                        Message = "Publicación no encontrada"
                    };
                }

                // Validar que el usuario no haya dejado ya un comentario o calificación
                var existingFeedback = await _context.Feedbacks
                    .FirstOrDefaultAsync(f => f.PublicationId == publicationId && f.UserId == dto.UserId);
                if (existingFeedback != null)
                {
                    return new ResponseDto<FeedbackDto>
                    {
                        StatusCode = HttpStatusCode.BAD_REQUEST,
                        Status = false,
                        Message = "Ya has comentado o calificado esta publicación"
                    };
                }

              
                var feedbackEntity = dto.Adapt<FeedbackEntity>(); 
                feedbackEntity.PublicationId = publicationId;
                feedbackEntity.CreateDate= DateTime.UtcNow;

                _context.Feedbacks.Add(feedbackEntity);
                await _context.SaveChangesAsync();

               
                var feedbackDto = feedbackEntity.Adapt<FeedbackDto>();

                return new ResponseDto<FeedbackDto>
                {
                    StatusCode = HttpStatusCode.CREATED,
                    Status = true,
                    Message = "Comentario o calificación creada correctamente",
                    Data = feedbackDto
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear feedback: {ex.Message}");
                return new ResponseDto<FeedbackDto>
                {
                    StatusCode = HttpStatusCode.INTERNAL_SERVER_ERROR,
                    Status = false,
                    Message = "Error interno al crear el comentario o calificación"
                };
            }
        }

        public async Task<ResponseDto<List<FeedbackDto>>> GetFeedbacksAsync(string publicationId)
        {
            try
            {
              
                var publicationExists = await _context.Publications.AnyAsync(p => p.Id == publicationId && p.IsEnabled);
                if (!publicationExists)
                {
                    return new ResponseDto<List<FeedbackDto>>
                    {
                        StatusCode = HttpStatusCode.NOT_FOUND,
                        Status = false,
                        Message = "Publicación no encontrada"
                    };
                }

                // Obtener los comentarios/feedbacks asociados a la publicación
                var feedbackEntities = await _context.Feedbacks
                    .Where(f => f.PublicationId == publicationId && f.IsEnabled)
                    .Include(f => f.User) // Incluir el usuario que hizo el comentario
                    .ToListAsync();

                
                var feedbackDtos = feedbackEntities.Select(feedback => new FeedbackDto
                {
                    Id = feedback.Id,
                    Comment = feedback.Comment,
                    Rating = feedback.Rating,
                    UserId = feedback.UserId,
                    UserName = $"{feedback.User.FirstName} {feedback.User.LastName}",
                    CreatedAt = DateTime.UtcNow,
                }).ToList();

                return new ResponseDto<List<FeedbackDto>>
                {
                    StatusCode = HttpStatusCode.OK,
                    Status = true,
                    Message = "Comentarios encontrados correctamente",
                    Data = feedbackDtos
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener comentarios: {ex.Message}");
                return new ResponseDto<List<FeedbackDto>>
                {
                    StatusCode = HttpStatusCode.INTERNAL_SERVER_ERROR,
                    Status = false,
                    Message = "Error interno al obtener los comentarios"
                };
            }
        }

        public async Task<ResponseDto<FeedbackDto>> EditFeedbackAsync(string feedbackId, FeedbackEditDto dto)
        {
            try
            {
                
                var feedbackEntity = await _context.Feedbacks
                    .FirstOrDefaultAsync(f => f.Id == feedbackId && f.IsEnabled);

                if (feedbackEntity == null)
                {
                    return new ResponseDto<FeedbackDto>
                    {
                        StatusCode = HttpStatusCode.NOT_FOUND,
                        Status = false,
                        Message = "Comentario no encontrado"
                    };
                }

                
                if (feedbackEntity.UserId != dto.UserId)
                {
                    return new ResponseDto<FeedbackDto>
                    {
                        StatusCode = HttpStatusCode.FORBIDDEN,
                        Status = false,
                        Message = "No tienes permiso para editar este comentario"
                    };
                }

               
                feedbackEntity.Comment = dto.Comment;
                feedbackEntity.Rating = dto.Rating;
                feedbackEntity.DateModified = DateTime.UtcNow;

                _context.Feedbacks.Update(feedbackEntity);
                await _context.SaveChangesAsync();

               
                var feedbackDto = feedbackEntity.Adapt<FeedbackDto>();

                return new ResponseDto<FeedbackDto>
                {
                    StatusCode = HttpStatusCode.OK,
                    Status = true,
                    Message = "Comentario editado correctamente",
                    Data = feedbackDto
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al editar comentario: {ex.Message}");
                return new ResponseDto<FeedbackDto>
                {
                    StatusCode = HttpStatusCode.INTERNAL_SERVER_ERROR,
                    Status = false,
                    Message = "Error interno al editar el comentario"
                };
            }
        }

        public async Task<ResponseDto<object>> DeleteFeedbackAsync(string feedbackId, string userId)
        {
            try
            {
              
                var feedbackEntity = await _context.Feedbacks
                    .FirstOrDefaultAsync(f => f.Id == feedbackId && f.IsEnabled);

                if (feedbackEntity == null)
                {
                    return new ResponseDto<object>
                    {
                        StatusCode = HttpStatusCode.NOT_FOUND,
                        Status = false,
                        Message = "Comentario no encontrado"
                    };
                }

                // Verificar si el usuario es el mismo que dejó el comentario original para lograr eliminarlo 
                if (feedbackEntity.UserId != userId)
                {
                    return new ResponseDto<object>
                    {
                        StatusCode = HttpStatusCode.FORBIDDEN,
                        Status = false,
                        Message = "No tienes permiso para eliminar este comentario"
                    };
                }

                // Marcar comentario como deshabilitado
                feedbackEntity.IsEnabled = false;
                _context.Feedbacks.Update(feedbackEntity);
                await _context.SaveChangesAsync();

                return new ResponseDto<object>
                {
                    StatusCode = HttpStatusCode.OK,
                    Status = true,
                    Message = "Comentario eliminado correctamente"
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar comentario: {ex.Message}");
                return new ResponseDto<object>
                {
                    StatusCode = HttpStatusCode.INTERNAL_SERVER_ERROR,
                    Status = false,
                    Message = "Error interno al eliminar el comentario"
                };
            }
        }


    }
}
