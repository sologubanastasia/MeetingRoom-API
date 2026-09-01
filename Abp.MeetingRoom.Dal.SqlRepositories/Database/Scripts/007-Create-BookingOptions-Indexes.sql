IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE [object_id] = OBJECT_ID(N'[meeting_room_anastasia].[BookingOptions]')
      AND [name] = N'IX_MeetingRoom_BookingOptions_RoomBookingId'
)
BEGIN
    CREATE INDEX [IX_MeetingRoom_BookingOptions_RoomBookingId]
        ON [meeting_room_anastasia].[BookingOptions] ([RoomBookingId]);
END;
IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE [object_id] = OBJECT_ID(N'[meeting_room_anastasia].[BookingOptions]')
      AND [name] = N'IX_MeetingRoom_BookingOptions_RoomOptionId'
)
BEGIN
    CREATE INDEX [IX_MeetingRoom_BookingOptions_RoomOptionId]
        ON [meeting_room_anastasia].[BookingOptions] ([RoomOptionId]);
END;
