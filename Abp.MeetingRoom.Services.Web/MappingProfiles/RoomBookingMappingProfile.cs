using Abp.MeetingRoom.Bll.Common.RoomBookings.Models;
using Abp.MeetingRoom.Services.Web.DTOs.RoomBookings;
using AutoMapper;

namespace Abp.MeetingRoom.Services.Web.MappingProfiles;

public sealed class RoomBookingMappingProfile : Profile
{
    public RoomBookingMappingProfile()
    {
        CreateMap<CreateRoomBookingRequest, RoomBooking>()
            .ForMember(destination => destination.Id, options => options.Ignore())
            .ForMember(destination => destination.Room, options => options.Ignore())
            .ForMember(destination => destination.RoomPrice, options => options.Ignore())
            .ForMember(destination => destination.OptionsPrice, options => options.Ignore())
            .ForMember(destination => destination.TotalPrice, options => options.Ignore())
            .ForMember(destination => destination.Status, options => options.Ignore())
            .ForMember(destination => destination.CreatedAt, options => options.Ignore())
            .ForMember(
                destination => destination.SelectedOptions,
                options =>
                    options.MapFrom(source =>
                        source.SelectedOptionIds.Select(id => new BookingOption
                        {
                            RoomOptionId = id,
                        })
                    )
            );

        CreateMap<BookingOption, BookingOptionResponse>();

        CreateMap<RoomBooking, RoomBookingResponse>()
            .ForMember(
                destination => destination.RoomName,
                options =>
                    options.MapFrom(source =>
                        source.Room == null ? string.Empty : source.Room.Name
                    )
            );
    }
}
