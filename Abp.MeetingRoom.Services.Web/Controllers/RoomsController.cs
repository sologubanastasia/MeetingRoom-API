using Abp.MeetingRoom.Bll.Common.Rooms;
using Abp.MeetingRoom.Bll.Common.Rooms.Models;
using Abp.MeetingRoom.Services.Web.DTOs.Rooms;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Abp.MeetingRoom.Services.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    private readonly IRoomManager _roomManager;
    private readonly IMapper _mapper;

    public RoomsController(IRoomManager roomManager, IMapper mapper)
    {
        _roomManager = roomManager;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var rooms = await _roomManager.GetAllRoomsAsync();
        return Ok(_mapper.Map<List<RoomResponse>>(rooms));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var room = await _roomManager.GetRoomByIdAsync(id);

        if (room == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<RoomResponse>(room));
    }

    [HttpGet("available")]
    public async Task<IActionResult> GetAvailable(
        [FromQuery] DateTime startTime,
        [FromQuery] DateTime endTime,
        [FromQuery] int capacity
    )
    {
        var rooms = await _roomManager.GetAvailableRoomsAsync(startTime, endTime, capacity);
        return Ok(_mapper.Map<List<RoomResponse>>(rooms));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRoomRequest request)
    {
        var model = _mapper.Map<Room>(request);
        var room = await _roomManager.CreateRoomAsync(model);
        var response = _mapper.Map<RoomResponse>(room);

        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRoomRequest request)
    {
        var model = _mapper.Map<Room>(request, options => options.Items["RoomId"] = id);
        var room = await _roomManager.UpdateRoomAsync(model);

        if (room == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<RoomResponse>(room));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _roomManager.DeleteRoomAsync(id);

        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
}
