namespace Abp.MeetingRoom.Dal.SqlRepositories.Database;
internal static class SqlObjectNames
{
    private const string Schema = "meeting_room_anastasia";
    internal static class Rooms
    {
        internal const string GetAll = $"[{Schema}].[usp_Rooms_GetAll]";
        internal const string GetById = $"[{Schema}].[usp_Rooms_GetById]";
        internal const string GetAvailable = $"[{Schema}].[usp_Rooms_GetAvailable]";
        internal const string Create = $"[{Schema}].[usp_Rooms_Create]";
        internal const string Update = $"[{Schema}].[usp_Rooms_Update]";
        internal const string SoftDelete = $"[{Schema}].[usp_Rooms_SoftDelete]";
    }
    internal static class RoomBookings
    {
        internal const string GetAll = $"[{Schema}].[usp_RoomBookings_GetAll]";
        internal const string GetById = $"[{Schema}].[usp_RoomBookings_GetById]";
        internal const string Create = $"[{Schema}].[usp_RoomBookings_Create]";
        internal const string Cancel = $"[{Schema}].[usp_RoomBookings_Cancel]";
    }
    internal static class Reports
    {
        internal const string GetRevenue = $"[{Schema}].[usp_Reports_GetRevenue]";
        internal const string GetPopularOptions =
            $"[{Schema}].[usp_Reports_GetPopularOptions]";
        internal const string GetRoomUsage = $"[{Schema}].[usp_Reports_GetRoomUsage]";
    }
    internal static class Types
    {
        internal const string GuidList = $"{Schema}.GuidList";
        internal const string RoomOptionInput = $"{Schema}.RoomOptionInput";
    }
}
