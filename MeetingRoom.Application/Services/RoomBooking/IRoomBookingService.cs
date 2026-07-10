using MeetingRoom.Application.Dtos.RoomBookings;

namespace MeetingRoom.Application.Services.RoomBooking
{
    /// <summary>
    /// Визначає операції для управління бронюваннями залів.
    /// </summary>
    public interface IRoomBookingService
    {
        /// <summary>
        /// Створює нове бронювання конференц-залу.
        /// </summary>
        /// <param name="request">Дані нового бронювання.</param>
        /// <returns>Створене бронювання.</returns>
        Task<RoomBookingResponse> CreateRoomBookingAsync(CreateRoomBookingRequest request);

        /// <summary>
        /// Повертає всі бронювання.
        /// </summary>
        /// <returns>Список бронювань.</returns>
        Task<List<RoomBookingResponse>> GetAllRoomBookingsAsync();

        /// <summary>
        /// Повертає бронювання за його ідентифікатором.
        /// </summary>
        /// <param name="id">Ідентифікатор бронювання.</param>
        /// <returns>
        /// Бронювання або null, якщо воно не знайдене.
        /// </returns>
        Task<RoomBookingResponse?> GetRoomBookingByIdAsync(Guid id);

        /// <summary>
        /// Скасовує бронювання.
        /// </summary>
        /// <param name="id">Ідентифікатор бронювання.</param>
        /// <returns>
        /// True, якщо бронювання успішно скасовано.
        /// </returns>
        Task<bool> CancelRoomBookingAsync(Guid id);

        /// <summary>
        /// Повертає бронювання, що перетинаються
        /// із заданим періодом.
        /// </summary>
        /// <param name="from">Початок періоду.</param>
        /// <param name="to">Кінець періоду.</param>
        /// <returns>Список бронювань.</returns>
        Task<List<RoomBookingResponse>> GetByPeriodAsync(DateTime from, DateTime to);
    }
}
