CREATE OR ALTER PROCEDURE [meeting_room_anastasia].[usp_RoomBookings_Cancel]
    @BookingId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE [meeting_room_anastasia].[RoomBookings] SET Status=2 WHERE Id=@BookingId AND Status=1;
    SELECT @@ROWCOUNT AS AffectedRows;
END;
