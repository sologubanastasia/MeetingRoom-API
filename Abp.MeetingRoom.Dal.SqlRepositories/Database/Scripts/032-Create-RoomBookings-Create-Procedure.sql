CREATE OR ALTER PROCEDURE [meeting_room_anastasia].[usp_RoomBookings_Create]
    @RoomId UNIQUEIDENTIFIER,@StartTime DATETIME2(7),@EndTime DATETIME2(7),
    @SelectedOptionIds [meeting_room_anastasia].[GuidList] READONLY
AS
BEGIN
    SET NOCOUNT ON; SET XACT_ABORT ON;
    IF @StartTime>=@EndTime THROW 50201,N'Start time must be earlier than end time.',1;
    IF @StartTime<SYSUTCDATETIME() THROW 50202,N'Start time cannot be in the past.',1;
    DECLARE @BookingId UNIQUEIDENTIFIER=NEWID(),@PricePerHour DECIMAL(18,2),
            @RoomPrice DECIMAL(18,2),@OptionsPrice DECIMAL(18,2);
    BEGIN TRY
        SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
        BEGIN TRANSACTION;
        SELECT @PricePerHour=PricePerHour FROM [meeting_room_anastasia].[Rooms] WITH(UPDLOCK,HOLDLOCK)
        WHERE Id=@RoomId AND IsDeleted=0;
        IF @PricePerHour IS NULL THROW 50203,N'Meeting room not found.',1;
        IF EXISTS(SELECT 1 FROM [meeting_room_anastasia].[RoomBookings]
                  WITH(UPDLOCK,HOLDLOCK,INDEX([IX_MeetingRoom_RoomBookings_ConflictSearch]))
                  WHERE RoomId=@RoomId AND Status=1 AND StartTime<@EndTime AND @StartTime<EndTime)
            THROW 50204,N'Meeting room is already booked for this time.',1;
        IF (SELECT COUNT(*) FROM @SelectedOptionIds)<>
           (SELECT COUNT(*) FROM [meeting_room_anastasia].[RoomOptions] o
            JOIN @SelectedOptionIds i ON i.Id=o.Id WHERE o.RoomId=@RoomId AND o.IsActive=1)
            THROW 50205,N'Some selected options are not available for this room.',1;
        SET @RoomPrice=[meeting_room_anastasia].[fn_CalculateRoomPrice](@PricePerHour,@StartTime,@EndTime);
        SELECT @OptionsPrice=COALESCE(SUM(o.Price),0)
        FROM [meeting_room_anastasia].[RoomOptions] o JOIN @SelectedOptionIds i ON i.Id=o.Id;
        INSERT [meeting_room_anastasia].[RoomBookings]
            (Id,RoomId,StartTime,EndTime,RoomPrice,OptionsPrice,TotalPrice,Status)
        VALUES(@BookingId,@RoomId,@StartTime,@EndTime,@RoomPrice,@OptionsPrice,@RoomPrice+@OptionsPrice,1);
        INSERT [meeting_room_anastasia].[BookingOptions]
            (Id,RoomBookingId,RoomOptionId,OptionName,OptionPrice)
        SELECT NEWID(),@BookingId,o.Id,o.Name,o.Price
        FROM [meeting_room_anastasia].[RoomOptions] o JOIN @SelectedOptionIds i ON i.Id=o.Id;
        COMMIT;
        SET TRANSACTION ISOLATION LEVEL READ COMMITTED;
    END TRY
    BEGIN CATCH
        IF XACT_STATE()<>0 ROLLBACK;
        SET TRANSACTION ISOLATION LEVEL READ COMMITTED;
        THROW;
    END CATCH;
    EXEC [meeting_room_anastasia].[usp_RoomBookings_GetById] @BookingId;
END;
