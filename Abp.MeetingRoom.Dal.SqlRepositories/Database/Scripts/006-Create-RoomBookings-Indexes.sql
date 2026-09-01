IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE [object_id] = OBJECT_ID(N'[meeting_room_anastasia].[RoomBookings]')
      AND [name] = N'IX_MeetingRoom_RoomBookings_ConflictSearch'
)
BEGIN
    CREATE INDEX [IX_MeetingRoom_RoomBookings_ConflictSearch]
        ON [meeting_room_anastasia].[RoomBookings] ([RoomId],[Status],[StartTime],[EndTime]);
END;
IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE [object_id] = OBJECT_ID(N'[meeting_room_anastasia].[RoomBookings]')
      AND [name] = N'IX_MeetingRoom_RoomBookings_ReportPeriod'
)
BEGIN
    CREATE INDEX [IX_MeetingRoom_RoomBookings_ReportPeriod]
        ON [meeting_room_anastasia].[RoomBookings] ([Status],[StartTime],[EndTime])
        INCLUDE ([RoomId],[RoomPrice],[OptionsPrice],[TotalPrice]);
END;
