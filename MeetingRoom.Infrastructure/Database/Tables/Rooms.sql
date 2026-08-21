CREATE TABLE [meeting_room_anastasia].[Rooms]
(
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [Name] NVARCHAR(100) NOT NULL,
    [Capacity] INT NOT NULL,
    [PricePerHour] DECIMAL(18,2) NOT NULL,
    [IsDeleted] BIT NOT NULL CONSTRAINT [DF_MeetingRoom_Rooms_IsDeleted] DEFAULT (0),
    [CreatedAt] DATETIME2(7) NOT NULL CONSTRAINT [DF_MeetingRoom_Rooms_CreatedAt] DEFAULT (SYSUTCDATETIME()),
    CONSTRAINT [PK_MeetingRoom_Rooms] PRIMARY KEY CLUSTERED ([Id]),
    CONSTRAINT [CK_MeetingRoom_Rooms_Name_NotBlank] CHECK (LEN(LTRIM(RTRIM([Name]))) > 0),
    CONSTRAINT [CK_MeetingRoom_Rooms_Capacity] CHECK ([Capacity] > 0),
    CONSTRAINT [CK_MeetingRoom_Rooms_PricePerHour] CHECK ([PricePerHour] > 0)
);
