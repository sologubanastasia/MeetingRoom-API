using Abp.MeetingRoom.Bll.Common.Rooms;
using Abp.MeetingRoom.Bll.Common.Rooms.Models;
using Abp.MeetingRoom.Services.Web.DTOs.Rooms;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Abp.MeetingRoom.Services.Web.Controllers;

/// <summary>
/// Надає HTTP-операції для керування конференц-залами.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    private readonly IRoomManager _roomManager;
    private readonly IMapper _mapper;

    /// <summary>
    /// Ініціалізує новий екземпляр контролера конференц-залів.
    /// </summary>
    /// <param name="roomManager">Менеджер бізнес-операцій конференц-залів.</param>
    /// <param name="mapper">Мапер між моделями бізнес-рівня та DTO.</param>
    public RoomsController(IRoomManager roomManager, IMapper mapper)
    {
        _roomManager = roomManager;
        _mapper = mapper;
    }

    /// <summary>
    /// Отримує всі активні конференц-зали.
    /// </summary>
    /// <returns>HTTP-відповідь зі списком конференц-залів.</returns>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var rooms = await _roomManager.GetAllRoomsAsync();
        return Ok(_mapper.Map<List<RoomResponse>>(rooms));
    }

    /// <summary>
    /// Отримує конференц-зал за його ідентифікатором.
    /// </summary>
    /// <param name="id">Унікальний ідентифікатор залу.</param>
    /// <returns>HTTP-відповідь із залом або статусом 404, якщо зал не знайдено.</returns>
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

    /// <summary>
    /// Отримує зали, доступні для бронювання у вказаний період.
    /// </summary>
    /// <param name="startTime">Дата й час початку періоду в UTC.</param>
    /// <param name="endTime">Дата й час завершення періоду в UTC.</param>
    /// <param name="capacity">Мінімальна необхідна місткість залу.</param>
    /// <returns>HTTP-відповідь зі списком доступних конференц-залів.</returns>
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

    /// <summary>
    /// Створює новий конференц-зал.
    /// </summary>
    /// <param name="request">Дані нового конференц-залу.</param>
    /// <returns>HTTP-відповідь зі створеним залом і адресою ресурсу.</returns>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRoomRequest request)
    {
        var model = _mapper.Map<Room>(request);
        var room = await _roomManager.CreateRoomAsync(model);
        var response = _mapper.Map<RoomResponse>(room);

        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    /// <summary>
    /// Оновлює наявний конференц-зал.
    /// </summary>
    /// <param name="id">Унікальний ідентифікатор залу.</param>
    /// <param name="request">Актуальні дані конференц-залу.</param>
    /// <returns>HTTP-відповідь з оновленим залом або статусом 404.</returns>
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

    /// <summary>
    /// Виконує логічне видалення конференц-залу.
    /// </summary>
    /// <param name="id">Унікальний ідентифікатор залу.</param>
    /// <returns>HTTP-відповідь зі статусом 204 або 404, якщо зал не знайдено.</returns>
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
