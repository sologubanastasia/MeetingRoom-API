CREATE OR ALTER PROCEDURE [meeting_room_anastasia].[usp_Rooms_Create]
    @Name NVARCHAR(100), @Capacity INT, @PricePerHour DECIMAL(18,2),
    @Options [meeting_room_anastasia].[RoomOptionInput] READONLY
AS
BEGIN
    SET NOCOUNT ON; SET XACT_ABORT ON;
    IF LEN(LTRIM(RTRIM(@Name)))=0 THROW 50103,N'Room name is required.',1;
    IF @Capacity<=0 THROW 50104,N'Capacity must be greater than zero.',1;
    IF @PricePerHour<=0 THROW 50105,N'Price per hour must be greater than zero.',1;
    DECLARE @RoomId UNIQUEIDENTIFIER=NEWID();
    BEGIN TRY
        BEGIN TRANSACTION;
        INSERT [meeting_room_anastasia].[Rooms](Id,Name,Capacity,PricePerHour,IsDeleted)
        VALUES(@RoomId,LTRIM(RTRIM(@Name)),@Capacity,@PricePerHour,0);
        INSERT [meeting_room_anastasia].[RoomOptions](Id,RoomId,Name,Price,IsActive)
        SELECT NEWID(),@RoomId,LTRIM(RTRIM(Name)),Price,1 FROM @Options;
        COMMIT;
    END TRY BEGIN CATCH IF XACT_STATE()<>0 ROLLBACK; THROW; END CATCH;
    EXEC [meeting_room_anastasia].[usp_Rooms_GetById] @RoomId;
END;
