using MeetingRoom.Application.Dtos.RoomBookings;
using MeetingRoom.Application.Services.RoomBooking;
using Microsoft.AspNetCore.Mvc;

namespace MeetingRoom.WebApi.Controllers
{
    /// <summary>
    /// Надає API для управління бронюваннями конференц-залів.
    /// </summary>
    [ApiController]
    [Route("api/room-bookings")]
    public class RoomBookingsController : ControllerBase
    {
        private readonly IRoomBookingService _service;

        /// <summary>
        /// Ініціалізує контролер бронювань.
        /// </summary>
        /// <param name="service">Сервіс управління бронюваннями.</param>
        public RoomBookingsController(IRoomBookingService service)
        {
            _service = service;
        }

        /// <summary>
        /// Створює нове бронювання конференц-залу.
        /// </summary>
        /// <param name="request">
        /// Дані про зал, час бронювання та вибрані послуги.
        /// </param>
        /// <returns>
        /// Створене бронювання з розрахованою загальною вартістю.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRoomBookingRequest request)
        {
            var booking = await _service.CreateRoomBookingAsync(request);

            return Ok(booking);
        }

        /// <summary>
        /// Повертає список усіх бронювань.
        /// </summary>
        /// <returns>Список бронювань конференц-залів.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var bookings = await _service.GetAllRoomBookingsAsync();

            return Ok(bookings);
        }

        /// <summary>
        /// Повертає бронювання за його унікальним ідентифікатором.
        /// </summary>
        /// <param name="id">Унікальний ID бронювання.</param>
        /// <returns>
        /// Дані бронювання або статус 404, якщо бронювання не знайдено.
        /// </returns>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var booking = await _service.GetRoomBookingByIdAsync(id);

            if (booking == null)
            {
                return NotFound();
            }

            return Ok(booking);
        }

        /// <summary>
        /// Скасовує бронювання за його унікальним ідентифікатором.
        /// </summary>
        /// <param name="id">Унікальний ID бронювання.</param>
        /// <returns>
        /// Статус 204 після успішного скасування
        /// або 404, якщо бронювання не знайдено.
        /// </returns>
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
