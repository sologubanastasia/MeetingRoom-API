IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE [object_id] = OBJECT_ID(N'[meeting_room_anastasia].[RoomOptions]')
      AND [name] = N'IX_MeetingRoom_RoomOptions_RoomId'
)
BEGIN
    CREATE INDEX [IX_MeetingRoom_RoomOptions_RoomId]
        ON [meeting_room_anastasia].[RoomOptions] ([RoomId]);
END;
IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE [object_id] = OBJECT_ID(N'[meeting_room_anastasia].[RoomOptions]')
      AND [name] = N'UX_MeetingRoom_RoomOptions_RoomId_Name_Active'
)
BEGIN
    CREATE UNIQUE INDEX [UX_MeetingRoom_RoomOptions_RoomId_Name_Active]
        ON [meeting_room_anastasia].[RoomOptions] ([RoomId],[Name])
        WHERE [IsActive] = 1;
END;
