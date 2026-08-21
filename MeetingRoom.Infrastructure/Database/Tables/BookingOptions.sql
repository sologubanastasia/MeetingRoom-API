CREATE TABLE [meeting_room_anastasia].[BookingOptions]
(
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [RoomBookingId] UNIQUEIDENTIFIER NOT NULL,
    [RoomOptionId] UNIQUEIDENTIFIER NOT NULL,
    [OptionName] NVARCHAR(100) NOT NULL,
    [OptionPrice] DECIMAL(18,2) NOT NULL,
    CONSTRAINT [PK_MeetingRoom_BookingOptions] PRIMARY KEY CLUSTERED ([Id]),
    CONSTRAINT [FK_MeetingRoom_BookingOptions_RoomBookings] FOREIGN KEY ([RoomBookingId])
        REFERENCES [meeting_room_anastasia].[RoomBookings] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_MeetingRoom_BookingOptions_RoomOptions] FOREIGN KEY ([RoomOptionId])
        REFERENCES [meeting_room_anastasia].[RoomOptions] ([Id]),
    CONSTRAINT [CK_MeetingRoom_BookingOptions_Name_NotBlank] CHECK (LEN(LTRIM(RTRIM([OptionName]))) > 0),
    CONSTRAINT [CK_MeetingRoom_BookingOptions_Price] CHECK ([OptionPrice] >= 0),
    CONSTRAINT [UQ_MeetingRoom_BookingOptions_Booking_Option] UNIQUE ([RoomBookingId],[RoomOptionId])
);
