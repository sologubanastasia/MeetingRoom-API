CREATE OR ALTER PROCEDURE [meeting_room_anastasia].[usp_Rooms_Update]
    @RoomId UNIQUEIDENTIFIER,@Name NVARCHAR(100),@Capacity INT,@PricePerHour DECIMAL(18,2),
    @Options [meeting_room_anastasia].[RoomOptionInput] READONLY
AS
BEGIN
    SET NOCOUNT ON; SET XACT_ABORT ON;
    IF LEN(LTRIM(RTRIM(@Name)))=0 THROW 50106,N'Room name is required.',1;
    IF @Capacity<=0 OR @PricePerHour<=0 THROW 50107,N'Capacity and price must be positive.',1;
    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE [meeting_room_anastasia].[Rooms]
        SET Name=LTRIM(RTRIM(@Name)),Capacity=@Capacity,PricePerHour=@PricePerHour
        WHERE Id=@RoomId AND IsDeleted=0;
        IF @@ROWCOUNT=0 THROW 50108,N'Meeting room not found.',1;
        UPDATE [meeting_room_anastasia].[RoomOptions] SET IsActive=0 WHERE RoomId=@RoomId AND IsActive=1;
        INSERT [meeting_room_anastasia].[RoomOptions](Id,RoomId,Name,Price,IsActive)
        SELECT NEWID(),@RoomId,LTRIM(RTRIM(Name)),Price,1 FROM @Options;
        COMMIT;
    END TRY BEGIN CATCH IF XACT_STATE()<>0 ROLLBACK; THROW; END CATCH;
    EXEC [meeting_room_anastasia].[usp_Rooms_GetById] @RoomId;
END;
