using MeetingRoom.Application.Dtos.Rooms;
using MeetingRoom.Application.Services.Rooms;
using Microsoft.AspNetCore.Mvc;

namespace MeetingRoom.WebApi.Controllers
{
    /// <summary>
    /// Надає API для управління конференц-залами.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomService _roomService;

        /// <summary>
        /// Ініціалізує контролер конференц-залів.
        /// </summary>
        /// <param name="roomService">Сервіс управління залами.</param>
        public RoomsController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        /// <summary>
        /// Повертає список усіх доступних конференц-залів.
        /// </summary>
        /// <returns>Список конференц-залів.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var rooms = await _roomService.GetAllRoomsAsync();

            return Ok(rooms);
        }

        /// <summary>
        /// Повертає конференц-зал за його унікальним ідентифікатором.
        /// </summary>
        /// <param name="id">Унікальний ID залу.</param>
        /// <returns>
        /// Дані залу або статус 404, якщо зал не знайдено.
        /// </returns>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var room = await _roomService.GetRoomByIdAsync(id);

            if (room == null)
            {
                return NotFound();
            }

            return Ok(room);
        }

        [HttpGet("available")]
        public async Task<IActionResult> GetAvailable(
            [FromQuery] DateTime startTime,
            [FromQuery] DateTime endTime,
            [FromQuery] int capacity
        )
        {
            var rooms = await _roomService.GetAvailableRoomsAsync(startTime, endTime, capacity);

            return Ok(rooms);
        }

        /// <summary>
        /// Створює новий конференц-зал.
        /// </summary>
        /// <param name="request">
        /// Назва, місткість, погодинна вартість і список послуг.
        /// </param>
        /// <returns>Створений конференц-зал з унікальним ID.</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRoomRequest request)
        {
            var room = await _roomService.CreateRoomAsync(request);

            return CreatedAtAction(nameof(GetById), new { id = room.Id }, room);
        }

        /// <summary>
        /// Оновлює інформацію про конференц-зал.
        /// </summary>
        /// <param name="id">Унікальний ID залу.</param>
        /// <param name="request">
        /// Оновлена назва, місткість, ціна та список послуг.
        /// </param>
        /// <returns>Оновлені дані конференц-залу.</returns>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRoomRequest request)
        {
            var room = await _roomService.UpdateRoomAsync(id, request);

            if (room == null)
            {
                return NotFound();
            }

            return Ok(room);
        }

        /// <summary>
        /// Видаляє конференц-зал за його унікальним ідентифікатором.
        /// </summary>
        /// <param name="id">Унікальний ID залу.</param>
        /// <returns>
        /// Статус 204 після успішного видалення
        /// або 404, якщо зал не знайдено.
        /// </returns>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _roomService.DeleteRoomAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
