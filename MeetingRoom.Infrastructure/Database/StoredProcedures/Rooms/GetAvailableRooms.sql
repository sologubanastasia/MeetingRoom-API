CREATE OR ALTER PROCEDURE [meeting_room_anastasia].[usp_Rooms_GetAvailable]
    @StartTime DATETIME2(7), @EndTime DATETIME2(7), @Capacity INT
AS
BEGIN
    SET NOCOUNT ON;
    IF @StartTime>=@EndTime THROW 50101,N'Start time must be earlier than end time.',1;
    IF @Capacity<=0 THROW 50102,N'Capacity must be greater than zero.',1;
    SELECT r.Id,r.Name,r.Capacity,r.PricePerHour,r.CreatedAt,
           o.Id AS OptionId,o.Name AS OptionName,o.Price AS OptionPrice
    FROM [meeting_room_anastasia].[Rooms] r
    LEFT JOIN [meeting_room_anastasia].[RoomOptions] o ON o.RoomId=r.Id AND o.IsActive=1
    WHERE r.IsDeleted=0 AND r.Capacity>=@Capacity
      AND NOT EXISTS (SELECT 1 FROM [meeting_room_anastasia].[RoomBookings] b
                      WHERE b.RoomId=r.Id AND b.Status=1
                        AND b.StartTime<@EndTime AND @StartTime<b.EndTime)
    ORDER BY r.Name,o.Name;
END;
