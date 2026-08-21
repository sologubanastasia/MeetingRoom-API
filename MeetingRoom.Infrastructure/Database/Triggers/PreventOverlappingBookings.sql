CREATE OR ALTER TRIGGER [meeting_room_anastasia].[TR_MeetingRoom_RoomBookings_PreventOverlap]
ON [meeting_room_anastasia].[RoomBookings]
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    IF EXISTS
    (
        SELECT 1
        FROM inserted n
        JOIN [meeting_room_anastasia].[RoomBookings] e
          WITH (UPDLOCK,HOLDLOCK,INDEX([IX_MeetingRoom_RoomBookings_ConflictSearch]))
          ON e.RoomId=n.RoomId AND e.Status=1 AND n.Status=1 AND e.Id<>n.Id
         AND e.StartTime<n.EndTime AND n.StartTime<e.EndTime
    )
        THROW 50010,N'Meeting room is already booked for this time.',1;
END;
