using Abp.MeetingRoom.Bll.Common.RoomBookings;
using Abp.MeetingRoom.Bll.Common.RoomBookings.Models;
namespace Abp.MeetingRoom.Bll.RoomBookings;
public sealed class RoomBookingManager : IRoomBookingManager
{
    private readonly IRoomBookingRepository _roomBookingRepository;
    public RoomBookingManager(IRoomBookingRepository roomBookingRepository)
    {
        _roomBookingRepository = roomBookingRepository;
    }
    public async Task<RoomBooking> CreateRoomBookingAsync(
        RoomBooking booking,
        CancellationToken cancellationToken
    )
    {
        if (booking.StartTime >= booking.EndTime)
        {
            throw new ArgumentException("Start time must be earlier than end time.");
        }
        var selectedOptionIds = booking.SelectedOptions
            .Select(option => option.RoomOptionId)
            .ToArray();
        return await _roomBookingRepository.CreateAsync(
            booking.RoomId,
            booking.StartTime,
            booking.EndTime,
            selectedOptionIds,
            cancellationToken
        );
    }
    public async Task<IReadOnlyList<RoomBooking>> GetAllRoomBookingsAsync(
        CancellationToken cancellationToken
    )
    {
        return await _roomBookingRepository.GetAllAsync(cancellationToken);
    }
    public async Task<RoomBooking?> GetRoomBookingByIdAsync(
        Guid id,
        CancellationToken cancellationToken
    )
    {
        return await _roomBookingRepository.GetByIdAsync(id, cancellationToken);
    }
    public async Task<bool> CancelRoomBookingAsync(
        Guid id,
        CancellationToken cancellationToken
    )
    {
        return await _roomBookingRepository.CancelAsync(id, cancellationToken);
    }
    public async Task<IReadOnlyList<RoomBooking>> GetByPeriodAsync(
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken
    )
    {
        ValidatePeriod(from, to);
        return await _roomBookingRepository.GetByPeriodAsync(from, to, cancellationToken);
    }
    public async Task<IReadOnlyList<RoomBooking>> GetActiveByPeriodAsync(
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken
    )
    {
        ValidatePeriod(from, to);
        return await _roomBookingRepository.GetActiveByPeriodAsync(
            from,
            to,
            cancellationToken
        );
    }
    private static void ValidatePeriod(DateTime from, DateTime to)
    {
        if (from >= to)
        {
            throw new ArgumentException("From date must be earlier than to date.");
        }
    }
}
