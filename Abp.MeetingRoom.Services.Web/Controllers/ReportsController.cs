using Abp.MeetingRoom.Bll.Common.Reports;
using Abp.MeetingRoom.Services.Web.Reports.Mappings;
using Microsoft.AspNetCore.Mvc;
namespace Abp.MeetingRoom.Services.Web.Controllers
{
    [ApiController]
    [Route("api/reports")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportManager _service;
        public ReportsController(IReportManager service)
        {
            _service = service;
        }
        [HttpGet("revenue")]
        public async Task<IActionResult> GetRevenue(
            [FromQuery] DateTime from,
            [FromQuery] DateTime to
        )
        {
            var report = await _service.GetRevenueReportAsync(from, to);
            return Ok(ReportMapper.ToResponse(report));
        }
        [HttpGet("popular-options")]
        public async Task<IActionResult> GetPopularOptions(
            [FromQuery] DateTime from,
            [FromQuery] DateTime to
        )
        {
            var report = await _service.GetPopularOptionsReportAsync(from, to);
            return Ok(report.Select(ReportMapper.ToResponse));
        }
        [HttpGet("room-usage")]
        public async Task<IActionResult> GetRoomUsage(
            [FromQuery] DateTime from,
            [FromQuery] DateTime to
        )
        {
            var report = await _service.GetRoomUsageReportAsync(from, to);
            return Ok(report.Select(ReportMapper.ToResponse));
        }
    }
}
