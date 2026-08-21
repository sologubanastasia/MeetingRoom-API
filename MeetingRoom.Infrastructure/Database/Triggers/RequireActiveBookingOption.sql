CREATE OR ALTER TRIGGER [meeting_room_anastasia].[TR_MeetingRoom_BookingOptions_RequireActive]
ON [meeting_room_anastasia].[BookingOptions]
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    IF EXISTS
    (
        SELECT 1
        FROM inserted i
        JOIN [meeting_room_anastasia].[RoomOptions] o ON o.Id=i.RoomOptionId
        WHERE o.IsActive=0
    )
        THROW 50012,N'Inactive room option cannot be added to a booking.',1;
END;
