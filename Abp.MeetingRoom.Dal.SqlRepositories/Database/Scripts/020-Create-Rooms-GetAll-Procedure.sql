CREATE OR ALTER PROCEDURE [meeting_room_anastasia].[usp_Rooms_GetAll]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT r.Id,r.Name,r.Capacity,r.PricePerHour,r.CreatedAt,
           o.Id AS OptionId,o.Name AS OptionName,o.Price AS OptionPrice
    FROM [meeting_room_anastasia].[Rooms] r
    LEFT JOIN [meeting_room_anastasia].[RoomOptions] o ON o.RoomId=r.Id AND o.IsActive=1
    WHERE r.IsDeleted=0 ORDER BY r.Name,o.Name;
END;
