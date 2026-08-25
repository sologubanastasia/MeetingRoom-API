using Abp.MeetingRoom.Bll.Common.RoomBookings;
using Abp.MeetingRoom.Services.Web.RoomBookings.Dtos;
using Abp.MeetingRoom.Services.Web.RoomBookings.Mappings;
using Microsoft.AspNetCore.Mvc;
namespace Abp.MeetingRoom.Services.Web.Controllers
{
    [ApiController]
    [Route("api/room-bookings")]
    public class RoomBookingsController : ControllerBase
    {
        private readonly IRoomBookingManager _service;
        public RoomBookingsController(IRoomBookingManager service)
        {
            _service = service;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRoomBookingRequest request)
        {
            var booking = await _service.CreateRoomBookingAsync(
                RoomBookingMapper.ToModel(request)
            );
            var response = RoomBookingMapper.ToResponse(booking);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var bookings = await _service.GetAllRoomBookingsAsync();
            return Ok(RoomBookingMapper.ToResponses(bookings));
        }
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var booking = await _service.GetRoomBookingByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            return Ok(RoomBookingMapper.ToResponse(booking));
        }
        [HttpPatch("{id:guid}/cancel")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            var result = await _service.CancelRoomBookingAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
