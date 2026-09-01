CREATE OR ALTER PROCEDURE [meeting_room_anastasia].[usp_Rooms_SoftDelete]
    @RoomId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE [meeting_room_anastasia].[Rooms] SET IsDeleted=1 WHERE Id=@RoomId AND IsDeleted=0;
    SELECT @@ROWCOUNT AS AffectedRows;
END;
