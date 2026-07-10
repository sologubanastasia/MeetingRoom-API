using MeetingRoom.Application.Services.Report;
using Microsoft.AspNetCore.Mvc;

namespace MeetingRoom.WebApi.Controllers
{
    /// <summary>
    /// Надає API для формування звітів та аналітики.
    /// </summary>
    [ApiController]
    [Route("api/reports")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _service;

        /// <summary>
        /// Ініціалізує контролер звітів.
        /// </summary>
        /// <param name="service">Сервіс формування звітів.</param>
        public ReportsController(IReportService service)
        {
            _service = service;
        }

        /// <summary>
        /// Формує звіт про дохід за вказаний період.
        /// </summary>
        /// <param name="from">Початок періоду.</param>
        /// <param name="to">Кінець періоду.</param>
        /// <returns>
        /// Кількість бронювань, дохід від залів,
        /// додаткових послуг і загальний дохід.
        /// </returns>
        [HttpGet("revenue")]
        public async Task<IActionResult> GetRevenue(
            [FromQuery] DateTime from,
            [FromQuery] DateTime to
        )
        {
            var report = await _service.GetRevenueReportAsync(from, to);

            return Ok(report);
        }

        /// <summary>
        /// Формує звіт про найпопулярніші додаткові послуги.
        /// </summary>
        /// <param name="from">Початок періоду.</param>
        /// <param name="to">Кінець періоду.</param>
        /// <returns>
        /// Список послуг із кількістю використань і отриманим доходом.
        /// </returns>
        [HttpGet("popular-options")]
        public async Task<IActionResult> GetPopularOptions(
            [FromQuery] DateTime from,
            [FromQuery] DateTime to
        )
        {
            var report = await _service.GetPopularOptionsReportAsync(from, to);

            return Ok(report);
        }

        /// <summary>
        /// Формує звіт про використання конференц-залів.
        /// </summary>
        /// <param name="from">Початок періоду.</param>
        /// <param name="to">Кінець періоду.</param>
        /// <returns>
        /// Дані про кількість бронювань, заброньовані години
        /// та дохід для кожного залу.
        /// </returns>
        [HttpGet("room-usage")]
        public async Task<IActionResult> GetRoomUsage(
            [FromQuery] DateTime from,
            [FromQuery] DateTime to
        )
        {
            var report = await _service.GetRoomUsageReportAsync(from, to);

            return Ok(report);
        }
    }
}
