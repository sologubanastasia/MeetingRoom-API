using AutoMapper;
using MeetingRoom.Application.Dtos.Rooms;
using MeetingRoom.Domain.Entities;

namespace MeetingRoom.Application.Mappings;

public class RoomMappingProfile : Profile
{
    public RoomMappingProfile()
    {
        CreateMap<RoomOption, RoomOptionResponse>();

        CreateMap<Room, RoomResponse>()
            .ForMember(
                destination => destination.Options,
                options => options.MapFrom(
                    source => source.Options.Where(option => option.IsActive)
                )
            );
    }
}
