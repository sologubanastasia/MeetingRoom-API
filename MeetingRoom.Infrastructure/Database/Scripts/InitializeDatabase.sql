:ON ERROR EXIT

SET NOCOUNT ON;
SET XACT_ABORT ON;
GO

IF SCHEMA_ID(N'meeting_room_anastasia') IS NULL
    THROW 50001,N'Schema meeting_room_anastasia does not exist. Ask the database administrator to create it.',1;
GO

:r ..\Tables\Rooms.sql
GO
:r ..\Tables\RoomOptions.sql
GO
:r ..\Tables\RoomBookings.sql
GO
:r ..\Tables\BookingOptions.sql
GO
:r ..\Indexes\RoomOptionsIndexes.sql
GO
:r ..\Indexes\RoomBookingsIndexes.sql
GO
:r ..\Indexes\BookingOptionsIndexes.sql
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
:r ..\Seeds\InitialData.sql
GO
