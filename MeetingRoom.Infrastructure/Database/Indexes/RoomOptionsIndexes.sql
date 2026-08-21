CREATE INDEX [IX_MeetingRoom_RoomOptions_RoomId]
    ON [meeting_room_anastasia].[RoomOptions] ([RoomId]);

CREATE UNIQUE INDEX [UX_MeetingRoom_RoomOptions_RoomId_Name_Active]
    ON [meeting_room_anastasia].[RoomOptions] ([RoomId],[Name])
    WHERE [IsActive] = 1;
