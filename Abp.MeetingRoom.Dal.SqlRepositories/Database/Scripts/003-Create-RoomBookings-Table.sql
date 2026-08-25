CREATE TABLE [meeting_room_anastasia].[RoomBookings]
(
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [RoomId] UNIQUEIDENTIFIER NOT NULL,
    [StartTime] DATETIME2(7) NOT NULL,
    [EndTime] DATETIME2(7) NOT NULL,
    [RoomPrice] DECIMAL(18,2) NOT NULL,
    [OptionsPrice] DECIMAL(18,2) NOT NULL,
    [TotalPrice] DECIMAL(18,2) NOT NULL CONSTRAINT [DF_MeetingRoom_RoomBookings_TotalPrice] DEFAULT (0),
    [Status] INT NOT NULL,
    [CreatedAt] DATETIME2(7) NOT NULL CONSTRAINT [DF_MeetingRoom_RoomBookings_CreatedAt] DEFAULT (SYSUTCDATETIME()),
    CONSTRAINT [PK_MeetingRoom_RoomBookings] PRIMARY KEY CLUSTERED ([Id]),
    CONSTRAINT [FK_MeetingRoom_RoomBookings_Rooms] FOREIGN KEY ([RoomId])
        REFERENCES [meeting_room_anastasia].[Rooms] ([Id]),
    CONSTRAINT [CK_MeetingRoom_RoomBookings_Time] CHECK ([EndTime] > [StartTime]),
    CONSTRAINT [CK_MeetingRoom_RoomBookings_RoomPrice] CHECK ([RoomPrice] >= 0),
    CONSTRAINT [CK_MeetingRoom_RoomBookings_OptionsPrice] CHECK ([OptionsPrice] >= 0),
    CONSTRAINT [CK_MeetingRoom_RoomBookings_TotalPrice] CHECK ([TotalPrice] >= 0),
    CONSTRAINT [CK_MeetingRoom_RoomBookings_Status] CHECK ([Status] IN (1,2)),
    CONSTRAINT [CK_MeetingRoom_RoomBookings_TotalPriceCalculation]
        CHECK ([TotalPrice] = [RoomPrice] + [OptionsPrice])
);
