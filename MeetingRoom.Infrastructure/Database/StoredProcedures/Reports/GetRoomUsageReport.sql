CREATE OR ALTER PROCEDURE [meeting_room_anastasia].[usp_Reports_GetRoomUsage]
    @From DATETIME2(7), @To DATETIME2(7)
AS
BEGIN
    SET NOCOUNT ON;
    IF @From>=@To THROW 50303,N'From must be earlier than To.',1;
    SELECT r.Name AS RoomName,COUNT_BIG(*) AS BookingsCount,
           SUM(CONVERT(DECIMAL(18,2),DATEDIFF_BIG(SECOND,b.StartTime,b.EndTime))/3600.0) AS BookedHours,
           SUM(b.TotalPrice) AS Revenue
    FROM [meeting_room_anastasia].[RoomBookings] b
    JOIN [meeting_room_anastasia].[Rooms] r ON r.Id=b.RoomId
    WHERE b.Status=1 AND b.StartTime<@To AND b.EndTime>@From
    GROUP BY r.Name ORDER BY BookingsCount DESC,r.Name;
END;
