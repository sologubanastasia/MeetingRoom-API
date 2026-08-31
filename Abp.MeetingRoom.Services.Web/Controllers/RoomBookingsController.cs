using Abp.MeetingRoom.Bll.Common.RoomBookings;
using Abp.MeetingRoom.Bll.Common.RoomBookings.Models;
using Abp.MeetingRoom.Services.Web.DTOs.RoomBookings;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Abp.MeetingRoom.Services.Web.Controllers;

[ApiController]
[Route("api/room-bookings")]
public class RoomBookingsController : ControllerBase
{
    private readonly IRoomBookingManager _roomBookingManager;
    private readonly IMapper _mapper;

    public RoomBookingsController(IRoomBookingManager roomBookingManager, IMapper mapper)
    {
        _roomBookingManager = roomBookingManager;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRoomBookingRequest request)
    {
        var model = _mapper.Map<RoomBooking>(request);
        var booking = await _roomBookingManager.CreateRoomBookingAsync(model);
        var response = _mapper.Map<RoomBookingResponse>(booking);

        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var bookings = await _roomBookingManager.GetAllRoomBookingsAsync();
        return Ok(_mapper.Map<List<RoomBookingResponse>>(bookings));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var booking = await _roomBookingManager.GetRoomBookingByIdAsync(id);

        if (booking == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<RoomBookingResponse>(booking));
    }

    [HttpPatch("{id:guid}/cancel")]
    public async Task<IActionResult> Cancel(Guid id)
    {
        var result = await _roomBookingManager.CancelRoomBookingAsync(id);

        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
}
