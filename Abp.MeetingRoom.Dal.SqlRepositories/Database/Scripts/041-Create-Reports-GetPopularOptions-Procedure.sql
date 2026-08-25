CREATE OR ALTER PROCEDURE [meeting_room_anastasia].[usp_Reports_GetPopularOptions]
    @From DATETIME2(7), @To DATETIME2(7)
AS
BEGIN
    SET NOCOUNT ON;
    IF @From>=@To THROW 50302,N'From must be earlier than To.',1;
    SELECT bo.OptionName,COUNT_BIG(*) AS UsageCount,SUM(bo.OptionPrice) AS Revenue
    FROM [meeting_room_anastasia].[BookingOptions] bo
    JOIN [meeting_room_anastasia].[RoomBookings] b ON b.Id=bo.RoomBookingId
    WHERE b.Status=1 AND b.StartTime<@To AND b.EndTime>@From
    GROUP BY bo.OptionName ORDER BY UsageCount DESC,bo.OptionName;
END;
