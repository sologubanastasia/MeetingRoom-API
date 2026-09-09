using Abp.MeetingRoom.Bll.Common.Reports;
using Abp.MeetingRoom.Services.Web.DTOs.Reports;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Abp.MeetingRoom.Services.Web.Controllers;

/// <summary>
/// Надає HTTP-операції для отримання звітів про роботу конференц-залів.
/// </summary>
[ApiController]
[Route("api/reports")]
public class ReportsController : ControllerBase
{
    private readonly IReportManager _reportManager;
    private readonly IMapper _mapper;

    /// <summary>
    /// Ініціалізує новий екземпляр контролера звітів.
    /// </summary>
    /// <param name="reportManager">Менеджер формування звітів.</param>
    /// <param name="mapper">Мапер між моделями бізнес-рівня та DTO.</param>
    public ReportsController(IReportManager reportManager, IMapper mapper)
    {
        _reportManager = reportManager;
        _mapper = mapper;
    }

    /// <summary>
    /// Отримує зведений звіт про дохід за вказаний період.
    /// </summary>
    /// <param name="from">Початок звітного періоду в UTC.</param>
    /// <param name="to">Завершення звітного періоду в UTC.</param>
    /// <returns>HTTP-відповідь зі зведеним звітом про дохід.</returns>
    [HttpGet("revenue")]
    public async Task<IActionResult> GetRevenue(
        [FromQuery] DateTime from,
        [FromQuery] DateTime to,
        CancellationToken cancellationToken
    )
    {
        var report = await _reportManager.GetRevenueReportAsync(
            from,
            to,
            cancellationToken
        );
        return Ok(_mapper.Map<RevenueReportResponse>(report));
    }

    /// <summary>
    /// Отримує звіт про популярність додаткових послуг.
    /// </summary>
    /// <param name="from">Початок звітного періоду в UTC.</param>
    /// <param name="to">Завершення звітного періоду в UTC.</param>
    /// <returns>HTTP-відповідь зі статистикою використання додаткових послуг.</returns>
    [HttpGet("popular-options")]
    public async Task<IActionResult> GetPopularOptions(
        [FromQuery] DateTime from,
        [FromQuery] DateTime to,
        CancellationToken cancellationToken
    )
    {
        var report = await _reportManager.GetPopularOptionsReportAsync(
            from,
            to,
            cancellationToken
        );
        return Ok(_mapper.Map<List<PopularOptionReportResponse>>(report));
    }

    /// <summary>
    /// Отримує звіт про використання конференц-залів.
    /// </summary>
    /// <param name="from">Початок звітного періоду в UTC.</param>
    /// <param name="to">Завершення звітного періоду в UTC.</param>
    /// <returns>HTTP-відповідь зі статистикою використання конференц-залів.</returns>
    [HttpGet("room-usage")]
    public async Task<IActionResult> GetRoomUsage(
        [FromQuery] DateTime from,
        [FromQuery] DateTime to,
        CancellationToken cancellationToken
    )
    {
        var report = await _reportManager.GetRoomUsageReportAsync(
            from,
            to,
            cancellationToken
        );
        return Ok(_mapper.Map<List<RoomUsageReportResponse>>(report));
    }
}
