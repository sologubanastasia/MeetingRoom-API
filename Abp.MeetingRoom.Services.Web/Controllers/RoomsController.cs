using Abp.MeetingRoom.Bll.Common.Rooms;
using Abp.MeetingRoom.Services.Web.Rooms.Dtos;
using Abp.MeetingRoom.Services.Web.Rooms.Mappings;
using Microsoft.AspNetCore.Mvc;
namespace Abp.MeetingRoom.Services.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomManager _roomManager;
        public RoomsController(IRoomManager roomManager)
        {
            _roomManager = roomManager;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var rooms = await _roomManager.GetAllRoomsAsync();
            return Ok(RoomMapper.ToResponses(rooms));
        }
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var room = await _roomManager.GetRoomByIdAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            return Ok(RoomMapper.ToResponse(room));
        }
        [HttpGet("available")]
        public async Task<IActionResult> GetAvailable(
            [FromQuery] DateTime startTime,
            [FromQuery] DateTime endTime,
            [FromQuery] int capacity
        )
        {
            var rooms = await _roomManager.GetAvailableRoomsAsync(startTime, endTime, capacity);
            return Ok(RoomMapper.ToResponses(rooms));
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRoomRequest request)
        {
            var room = await _roomManager.CreateRoomAsync(RoomMapper.ToModel(request));
            var response = RoomMapper.ToResponse(room);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRoomRequest request)
        {
            var room = await _roomManager.UpdateRoomAsync(RoomMapper.ToModel(id, request));
            if (room == null)
            {
                return NotFound();
            }
            return Ok(RoomMapper.ToResponse(room));
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
}
