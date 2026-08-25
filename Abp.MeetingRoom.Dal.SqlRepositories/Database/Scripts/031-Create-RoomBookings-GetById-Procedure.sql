CREATE OR ALTER PROCEDURE [meeting_room_anastasia].[usp_RoomBookings_GetById]
    @BookingId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    SELECT b.Id,b.RoomId,r.Name AS RoomName,b.StartTime,b.EndTime,b.RoomPrice,b.OptionsPrice,b.TotalPrice,b.Status,b.CreatedAt,
           bo.Id AS BookingOptionId,bo.RoomOptionId,bo.OptionName,bo.OptionPrice
    FROM [meeting_room_anastasia].[RoomBookings] b
    JOIN [meeting_room_anastasia].[Rooms] r ON r.Id=b.RoomId
    LEFT JOIN [meeting_room_anastasia].[BookingOptions] bo ON bo.RoomBookingId=b.Id
    WHERE b.Id=@BookingId ORDER BY bo.OptionName;
END;
