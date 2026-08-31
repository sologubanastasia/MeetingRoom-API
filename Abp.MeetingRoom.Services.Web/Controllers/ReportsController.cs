using Abp.MeetingRoom.Bll.Common.Reports;
using Abp.MeetingRoom.Services.Web.DTOs.Reports;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Abp.MeetingRoom.Services.Web.Controllers;

[ApiController]
[Route("api/reports")]
public class ReportsController : ControllerBase
{
    private readonly IReportManager _reportManager;
    private readonly IMapper _mapper;

    public ReportsController(IReportManager reportManager, IMapper mapper)
    {
        _reportManager = reportManager;
        _mapper = mapper;
    }

    [HttpGet("revenue")]
    public async Task<IActionResult> GetRevenue(
        [FromQuery] DateTime from,
        [FromQuery] DateTime to
    )
    {
        var report = await _reportManager.GetRevenueReportAsync(from, to);
        return Ok(_mapper.Map<RevenueReportResponse>(report));
    }

    [HttpGet("popular-options")]
    public async Task<IActionResult> GetPopularOptions(
        [FromQuery] DateTime from,
        [FromQuery] DateTime to
    )
    {
        var report = await _reportManager.GetPopularOptionsReportAsync(from, to);
        return Ok(_mapper.Map<List<PopularOptionReportResponse>>(report));
    }

    [HttpGet("room-usage")]
    public async Task<IActionResult> GetRoomUsage(
        [FromQuery] DateTime from,
        [FromQuery] DateTime to
    )
    {
        var report = await _reportManager.GetRoomUsageReportAsync(from, to);
        return Ok(_mapper.Map<List<RoomUsageReportResponse>>(report));
    }
}
