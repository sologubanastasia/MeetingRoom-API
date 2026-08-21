CREATE INDEX [IX_MeetingRoom_RoomBookings_ConflictSearch]
    ON [meeting_room_anastasia].[RoomBookings] ([RoomId],[Status],[StartTime],[EndTime]);

CREATE INDEX [IX_MeetingRoom_RoomBookings_ReportPeriod]
    ON [meeting_room_anastasia].[RoomBookings] ([Status],[StartTime],[EndTime])
    INCLUDE ([RoomId],[RoomPrice],[OptionsPrice],[TotalPrice]);
