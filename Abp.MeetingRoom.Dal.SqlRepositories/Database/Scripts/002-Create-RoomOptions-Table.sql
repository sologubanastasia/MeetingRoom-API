CREATE TABLE [meeting_room_anastasia].[RoomOptions]
(
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [RoomId] UNIQUEIDENTIFIER NOT NULL,
    [Name] NVARCHAR(100) NOT NULL,
    [Price] DECIMAL(18,2) NOT NULL,
    [IsActive] BIT NOT NULL CONSTRAINT [DF_MeetingRoom_RoomOptions_IsActive] DEFAULT (1),
    CONSTRAINT [PK_MeetingRoom_RoomOptions] PRIMARY KEY CLUSTERED ([Id]),
    CONSTRAINT [FK_MeetingRoom_RoomOptions_Rooms] FOREIGN KEY ([RoomId])
        REFERENCES [meeting_room_anastasia].[Rooms] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [CK_MeetingRoom_RoomOptions_Name_NotBlank] CHECK (LEN(LTRIM(RTRIM([Name]))) > 0),
    CONSTRAINT [CK_MeetingRoom_RoomOptions_Price] CHECK ([Price] >= 0)
);
