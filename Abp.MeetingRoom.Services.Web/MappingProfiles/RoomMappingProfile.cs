using Abp.MeetingRoom.Bll.Common.Rooms.Models;
using Abp.MeetingRoom.Services.Web.DTOs.Rooms;
using AutoMapper;

namespace Abp.MeetingRoom.Services.Web.MappingProfiles;

public sealed class RoomMappingProfile : Profile
{
    public RoomMappingProfile()
    {
        CreateMap<CreateRoomOptionRequest, RoomOption>()
            .ForMember(destination => destination.Id, options => options.Ignore())
            .ForMember(destination => destination.RoomId, options => options.Ignore())
            .ForMember(destination => destination.Room, options => options.Ignore())
            .ForMember(destination => destination.IsActive, options => options.MapFrom(_ => true))
            .ForMember(destination => destination.BookingOptions, options => options.Ignore());

        CreateMap<UpdateRoomOptionRequest, RoomOption>()
            .ForMember(destination => destination.Id, options => options.Ignore())
            .ForMember(destination => destination.RoomId, options => options.Ignore())
            .ForMember(destination => destination.Room, options => options.Ignore())
            .ForMember(destination => destination.IsActive, options => options.MapFrom(_ => true))
            .ForMember(destination => destination.BookingOptions, options => options.Ignore());

        CreateMap<CreateRoomRequest, Room>()
            .ForMember(destination => destination.Id, options => options.Ignore())
            .ForMember(destination => destination.IsDeleted, options => options.Ignore())
            .ForMember(destination => destination.CreatedAt, options => options.Ignore())
            .ForMember(destination => destination.Bookings, options => options.Ignore());

        CreateMap<UpdateRoomRequest, Room>()
            .ForMember(destination => destination.Id, options => options.Ignore())
            .ForMember(destination => destination.IsDeleted, options => options.Ignore())
            .ForMember(destination => destination.CreatedAt, options => options.Ignore())
            .ForMember(destination => destination.Bookings, options => options.Ignore())
            .AfterMap(
                (_, room, context) =>
                {
                    var roomId = (Guid)context.Items["RoomId"];
                    room.Id = roomId;

                    foreach (var option in room.Options)
                    {
                        option.RoomId = roomId;
                    }
                }
            );

        CreateMap<RoomOption, RoomOptionResponse>();

        CreateMap<Room, RoomResponse>()
            .ForMember(
                destination => destination.Options,
                options =>
                    options.MapFrom(source =>
                        source.Options.Where(option => option.IsActive)
                    )
            );
    }
}
