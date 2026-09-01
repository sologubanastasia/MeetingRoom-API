CREATE OR ALTER PROCEDURE [meeting_room_anastasia].[usp_Reports_GetRevenue]
    @From DATETIME2(7), @To DATETIME2(7)
AS
BEGIN
    SET NOCOUNT ON;
    IF @From>=@To THROW 50301,N'From must be earlier than To.',1;
    SELECT COUNT_BIG(*) AS BookingsCount,COALESCE(SUM(RoomPrice),0) AS RoomRevenue,
           COALESCE(SUM(OptionsPrice),0) AS OptionsRevenue,COALESCE(SUM(TotalPrice),0) AS TotalRevenue
    FROM [meeting_room_anastasia].[RoomBookings]
    WHERE Status=1 AND StartTime<@To AND EndTime>@From;
END;
