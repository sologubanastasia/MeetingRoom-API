CREATE OR ALTER PROCEDURE [meeting_room_anastasia].[usp_RoomBookings_GetAll]
    @From DATETIME2(7) = NULL,
    @To DATETIME2(7) = NULL,
    @Status INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    IF (@From IS NULL AND @To IS NOT NULL) OR (@From IS NOT NULL AND @To IS NULL)
        THROW 50206,N'Both period boundaries are required.',1;
    IF @From IS NOT NULL AND @From >= @To
        THROW 50207,N'From date must be earlier than to date.',1;
    IF @Status IS NOT NULL AND @Status NOT IN (1,2)
        THROW 50208,N'Unknown booking status.',1;
    SELECT b.Id,b.RoomId,r.Name AS RoomName,b.StartTime,b.EndTime,b.RoomPrice,b.OptionsPrice,b.TotalPrice,b.Status,b.CreatedAt,
           bo.Id AS BookingOptionId,bo.RoomOptionId,bo.OptionName,bo.OptionPrice
    FROM [meeting_room_anastasia].[RoomBookings] b
    JOIN [meeting_room_anastasia].[Rooms] r ON r.Id=b.RoomId
    LEFT JOIN [meeting_room_anastasia].[BookingOptions] bo ON bo.RoomBookingId=b.Id
    WHERE (@From IS NULL OR (b.StartTime<@To AND b.EndTime>@From))
      AND (@Status IS NULL OR b.Status=@Status)
    ORDER BY b.CreatedAt DESC,bo.OptionName;
END;
