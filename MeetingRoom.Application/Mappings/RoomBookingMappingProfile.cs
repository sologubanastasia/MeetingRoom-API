using AutoMapper;
using MeetingRoom.Application.Dtos.RoomBookings;
using MeetingRoom.Domain.Entities;

namespace MeetingRoom.Application.Mappings;

public class RoomBookingMappingProfile : Profile
{
    public RoomBookingMappingProfile()
    {
        CreateMap<BookingOption, BookingOptionResponse>();

        CreateMap<RoomBooking, RoomBookingResponse>()
            .ForMember(
                destination => destination.RoomName,
                options => options.MapFrom(
                    source => source.Room == null ? string.Empty : source.Room.Name
                )
            );
    }
}
