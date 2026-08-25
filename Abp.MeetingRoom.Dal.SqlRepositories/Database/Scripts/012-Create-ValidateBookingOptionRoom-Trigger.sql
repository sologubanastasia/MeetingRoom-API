CREATE OR ALTER TRIGGER [meeting_room_anastasia].[TR_MeetingRoom_BookingOptions_ValidateRoom]
ON [meeting_room_anastasia].[BookingOptions]
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    IF EXISTS
    (
        SELECT 1
        FROM inserted i
        JOIN [meeting_room_anastasia].[RoomBookings] b ON b.Id=i.RoomBookingId
        JOIN [meeting_room_anastasia].[RoomOptions] o ON o.Id=i.RoomOptionId
        WHERE o.RoomId<>b.RoomId
    )
        THROW 50011,N'Selected option does not belong to the booked room.',1;
END;
