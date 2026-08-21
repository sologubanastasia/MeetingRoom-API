:ON ERROR EXIT

SET NOCOUNT ON;
SET XACT_ABORT ON;
GO

IF SCHEMA_ID(N'meeting_room_anastasia') IS NULL
    THROW 50001,N'Schema meeting_room_anastasia does not exist.',1;
GO

IF OBJECT_ID(N'[meeting_room_anastasia].[Rooms]',N'U') IS NULL
   OR OBJECT_ID(N'[meeting_room_anastasia].[RoomOptions]',N'U') IS NULL
   OR OBJECT_ID(N'[meeting_room_anastasia].[RoomBookings]',N'U') IS NULL
   OR OBJECT_ID(N'[meeting_room_anastasia].[BookingOptions]',N'U') IS NULL
    THROW 50002,N'One or more required tables do not exist.',1;
GO

IF OBJECT_ID(N'[meeting_room_anastasia].[DF_MeetingRoom_Rooms_CreatedAt]',N'D') IS NULL
    EXEC(N'ALTER TABLE [meeting_room_anastasia].[Rooms] ADD CONSTRAINT [DF_MeetingRoom_Rooms_CreatedAt] DEFAULT(SYSUTCDATETIME()) FOR [CreatedAt];');

IF OBJECT_ID(N'[meeting_room_anastasia].[DF_MeetingRoom_RoomBookings_CreatedAt]',N'D') IS NULL
    EXEC(N'ALTER TABLE [meeting_room_anastasia].[RoomBookings] ADD CONSTRAINT [DF_MeetingRoom_RoomBookings_CreatedAt] DEFAULT(SYSUTCDATETIME()) FOR [CreatedAt];');

IF OBJECT_ID(N'[meeting_room_anastasia].[CK_MeetingRoom_RoomBookings_TotalPriceCalculation]',N'C') IS NULL
    EXEC(N'ALTER TABLE [meeting_room_anastasia].[RoomBookings] WITH CHECK ADD CONSTRAINT [CK_MeetingRoom_RoomBookings_TotalPriceCalculation] CHECK([TotalPrice]=[RoomPrice]+[OptionsPrice]);');
GO

IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE object_id=OBJECT_ID(N'[meeting_room_anastasia].[RoomOptions]') AND name=N'UX_MeetingRoom_RoomOptions_RoomId_Name_Active')
    EXEC(N'CREATE UNIQUE INDEX [UX_MeetingRoom_RoomOptions_RoomId_Name_Active] ON [meeting_room_anastasia].[RoomOptions]([RoomId],[Name]) WHERE [IsActive]=1;');

IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE object_id=OBJECT_ID(N'[meeting_room_anastasia].[RoomBookings]') AND name=N'IX_MeetingRoom_RoomBookings_ReportPeriod')
    EXEC(N'CREATE INDEX [IX_MeetingRoom_RoomBookings_ReportPeriod] ON [meeting_room_anastasia].[RoomBookings]([Status],[StartTime],[EndTime]) INCLUDE([RoomId],[RoomPrice],[OptionsPrice],[TotalPrice]);');
GO

:r ..\Types\GuidList.sql
GO
:r ..\Types\RoomOptionInput.sql
GO
:r ..\Functions\CalculateRoomPrice.sql
GO
:r ..\Triggers\PreventOverlappingBookings.sql
GO
:r ..\Triggers\ValidateBookingOptionRoom.sql
GO
:r ..\Triggers\RequireActiveBookingOption.sql
GO
:r ..\StoredProcedures\Rooms\GetAllRooms.sql
GO
:r ..\StoredProcedures\Rooms\GetRoomById.sql
GO
:r ..\StoredProcedures\Rooms\GetAvailableRooms.sql
GO
:r ..\StoredProcedures\Rooms\CreateRoom.sql
GO
:r ..\StoredProcedures\Rooms\UpdateRoom.sql
GO
:r ..\StoredProcedures\Rooms\SoftDeleteRoom.sql
GO
:r ..\StoredProcedures\RoomBookings\GetAllRoomBookings.sql
GO
:r ..\StoredProcedures\RoomBookings\GetRoomBookingById.sql
GO
:r ..\StoredProcedures\RoomBookings\CreateRoomBooking.sql
GO
:r ..\StoredProcedures\RoomBookings\CancelRoomBooking.sql
GO
:r ..\StoredProcedures\Reports\GetRevenueReport.sql
GO
:r ..\StoredProcedures\Reports\GetPopularOptionsReport.sql
GO
:r ..\StoredProcedures\Reports\GetRoomUsageReport.sql
GO
