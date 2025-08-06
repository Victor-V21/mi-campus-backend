using MiCampus.Constants;
using MiCampus.Database;
using MiCampus.Dtos.Common;
using MiCampus.Dtos.Statistics;
using MiCampus.Services.Interfaces;

namespace MiCampus.Services
{

    public class StatisticsService : IStatisticsService
        //Para el conteo de los registros, lo deje asi por que aun no tenemos la pagina para esto
    {
        //private readonly CampusDbContext _context;

        //public StatisticsService(CampusDbContext context)
        //{
        //    _context = context;
        //}

        //public async Task<ResponseDto<StatisticsDto>> GetCounts()
        //{
        //    var statistics = new StatisticsDto();

        //    statistics.EventosCount = await _context.Campuses
        //        .();

        //    return new ResponseDto<StatisticsDto>
        //    {
        //        StatusCode = HttpStatusCode.OK,
        //        Status = true,
        //        Message = "Datos obtenidos correctamente",
        //        Data = statistics
        //    };
        //}
    }
}
