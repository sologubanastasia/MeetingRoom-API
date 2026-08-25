using Abp.MeetingRoom.Bll.Common.Shared.Exceptions;
namespace Abp.MeetingRoom.Bll.Common.RoomBookings.Exceptions;
public sealed class BookingConflictException : BusinessRuleException
{
    public BookingConflictException(Exception innerException)
        : base("Meeting room is already booked for this time.", innerException) { }
}
