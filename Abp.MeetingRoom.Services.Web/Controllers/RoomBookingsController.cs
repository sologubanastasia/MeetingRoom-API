using Abp.MeetingRoom.Bll.Common.RoomBookings;
using Abp.MeetingRoom.Bll.Common.RoomBookings.Models;
using Abp.MeetingRoom.Services.Web.DTOs.RoomBookings;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Abp.MeetingRoom.Services.Web.Controllers;

/// <summary>
/// Надає HTTP-операції для керування бронюваннями конференц-залів.
/// </summary>
[ApiController]
[Route("api/room-bookings")]
public class RoomBookingsController : ControllerBase
{
    private readonly IRoomBookingManager _roomBookingManager;
    private readonly IMapper _mapper;

    /// <summary>
    /// Ініціалізує новий екземпляр контролера бронювань.
    /// </summary>
    /// <param name="roomBookingManager">Менеджер бізнес-операцій бронювань.</param>
    /// <param name="mapper">Мапер між моделями бізнес-рівня та DTO.</param>
    public RoomBookingsController(IRoomBookingManager roomBookingManager, IMapper mapper)
    {
        _roomBookingManager = roomBookingManager;
        _mapper = mapper;
    }

    /// <summary>
    /// Створює нове бронювання конференц-залу.
    /// </summary>
    /// <param name="request">Дані нового бронювання.</param>
    /// <returns>HTTP-відповідь зі створеним бронюванням і адресою ресурсу.</returns>
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateRoomBookingRequest request,
        CancellationToken cancellationToken
    )
    {
        var model = _mapper.Map<RoomBooking>(request);
        var booking = await _roomBookingManager.CreateRoomBookingAsync(
            model,
            cancellationToken
        );
        var response = _mapper.Map<RoomBookingResponse>(booking);

        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    /// <summary>
    /// Отримує всі бронювання конференц-залів.
    /// </summary>
    /// <returns>HTTP-відповідь зі списком бронювань.</returns>
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var bookings = await _roomBookingManager.GetAllRoomBookingsAsync(cancellationToken);
        return Ok(_mapper.Map<List<RoomBookingResponse>>(bookings));
    }

    /// <summary>
    /// Отримує бронювання за його ідентифікатором.
    /// </summary>
    /// <param name="id">Унікальний ідентифікатор бронювання.</param>
    /// <returns>HTTP-відповідь із бронюванням або статусом 404.</returns>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var booking = await _roomBookingManager.GetRoomBookingByIdAsync(
            id,
            cancellationToken
        );

        if (booking == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<RoomBookingResponse>(booking));
    }

    /// <summary>
    /// Скасовує бронювання конференц-залу.
    /// </summary>
    /// <param name="id">Унікальний ідентифікатор бронювання.</param>
    /// <returns>HTTP-відповідь зі статусом 204 або 404, якщо бронювання не знайдено.</returns>
    [HttpPatch("{id:guid}/cancel")]
    public async Task<IActionResult> Cancel(Guid id, CancellationToken cancellationToken)
    {
        var result = await _roomBookingManager.CancelRoomBookingAsync(
            id,
            cancellationToken
        );

        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
}
