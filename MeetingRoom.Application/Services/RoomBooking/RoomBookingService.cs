using MeetingRoom.Application.Dtos.RoomBookings;
using MeetingRoom.Application.Services.Price;
using MeetingRoom.Domain.Entities;
using MeetingRoom.Domain.Interfaces;

namespace MeetingRoom.Application.Services.RoomBooking
{
    /// <summary>
    /// Реалізує бізнес-логіку управління бронюваннями.
    /// </summary>
    public class RoomBookingService : IRoomBookingService
    {
        private readonly IRoomBookingRepository _roomBookingRepository;

        private readonly IRoomRepository _roomRepository;
        private readonly IPriceService _priceService;

        public RoomBookingService(
            IRoomBookingRepository roomBookingRepository,
            IRoomRepository roomRepository,
            IPriceService priceService
        )
        {
            _roomBookingRepository = roomBookingRepository;
            _roomRepository = roomRepository;
            _priceService = priceService;
        }

        public async Task<RoomBookingResponse> CreateRoomBookingAsync(
            CreateRoomBookingRequest request
        )
        {
            var room = await _roomRepository.GetByIdAsync(request.RoomId);

            if (room == null || room.IsDeleted)
            {
                throw new InvalidOperationException("Meeting room not found.");
            }

            if (request.StartTime >= request.EndTime)
            {
                throw new ArgumentException("Start time must be earlier than end time.");
            }

            var hasConflict = await _roomBookingRepository.HasTimeConflictAsync(
                request.RoomId,
                request.StartTime,
                request.EndTime
            );

            if (hasConflict)
            {
                throw new InvalidOperationException(
                    "Meeting room is already booked for this time."
                );
            }

            var selectedOptions = room
                .Options.Where(option =>
                    request.SelectedOptionIds.Contains(option.Id) && option.IsActive
                )
                .ToList();

            if (selectedOptions.Count != request.SelectedOptionIds.Count)
            {
                throw new InvalidOperationException(
                    "Some selected options are not available for this room."
                );
            }

            var roomPrice = _priceService.CalculateRoomPrice(
                room.PricePerHour,
                request.StartTime,
                request.EndTime
            );

            var optionsPrice = selectedOptions.Sum(option => option.Price);

            var booking = new Domain.Entities.RoomBooking
            {
                Id = Guid.NewGuid(),
                RoomId = room.Id,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                RoomPrice = roomPrice,
                OptionsPrice = optionsPrice,
                TotalPrice = roomPrice + optionsPrice,
                Status = BookingStatus.Active,
                CreatedAt = DateTime.UtcNow,

                SelectedOptions = selectedOptions
                    .Select(option => new BookingOption
                    {
                        Id = Guid.NewGuid(),
                        RoomOptionId = option.Id,
                        OptionName = option.Name,
                        OptionPrice = option.Price,
                    })
                    .ToList(),
            };

            await _roomBookingRepository.AddAsync(booking);
            await _roomBookingRepository.SaveChangesAsync();

            booking.Room = room;

            return MapToResponse(booking);
        }

        public async Task<List<RoomBookingResponse>> GetAllRoomBookingsAsync()
        {
            var bookings = await _roomBookingRepository.GetAllAsync();

            return bookings.Select(MapToResponse).ToList();
        }

        public async Task<RoomBookingResponse?> GetRoomBookingByIdAsync(Guid id)
        {
            var booking = await _roomBookingRepository.GetByIdAsync(id);

            return booking == null ? null : MapToResponse(booking);
        }

        public async Task<bool> CancelRoomBookingAsync(Guid id)
        {
            var booking = await _roomBookingRepository.GetByIdAsync(id);

            if (booking == null)
            {
                return false;
            }

            booking.Status = BookingStatus.Cancelled;

            _roomBookingRepository.Update(booking);
            await _roomBookingRepository.SaveChangesAsync();

            return true;
        }

        public async Task<List<RoomBookingResponse>> GetByPeriodAsync(DateTime from, DateTime to)
        {
            if (from >= to)
            {
                throw new ArgumentException("From date must be earlier than to date.");
            }

            var bookings = await _roomBookingRepository.GetByPeriodAsync(from, to);

            return bookings.Select(MapToResponse).ToList();
        }

        /// <summary>
        /// Перетворює сутність бронювання у DTO відповіді.
        /// </summary>
        private static RoomBookingResponse MapToResponse(Domain.Entities.RoomBooking booking)
        {
            return new RoomBookingResponse
            {
                Id = booking.Id,
                RoomId = booking.RoomId,
                RoomName = booking.Room?.Name ?? string.Empty,
                StartTime = booking.StartTime,
                EndTime = booking.EndTime,
                RoomPrice = booking.RoomPrice,
                OptionsPrice = booking.OptionsPrice,
                TotalPrice = booking.TotalPrice,
                Status = booking.Status,
                CreatedAt = booking.CreatedAt,

                SelectedOptions = booking
                    .SelectedOptions.Select(option => new BookingOptionResponse
                    {
                        Id = option.Id,
                        RoomOptionId = option.RoomOptionId,
                        OptionName = option.OptionName,
                        OptionPrice = option.OptionPrice,
                    })
                    .ToList(),
            };
        }
    }
}
