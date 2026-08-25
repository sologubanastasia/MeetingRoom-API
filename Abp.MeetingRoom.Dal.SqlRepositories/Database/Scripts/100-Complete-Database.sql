SET NOCOUNT ON;
SET XACT_ABORT ON;
GO
IF SCHEMA_ID(N'meeting_room_anastasia') IS NULL
    THROW 50001, N'Schema meeting_room_anastasia does not exist.', 1;
IF OBJECT_ID(N'[meeting_room_anastasia].[Rooms]', N'U') IS NULL
   OR OBJECT_ID(N'[meeting_room_anastasia].[RoomOptions]', N'U') IS NULL
   OR OBJECT_ID(N'[meeting_room_anastasia].[RoomBookings]', N'U') IS NULL
   OR OBJECT_ID(N'[meeting_room_anastasia].[BookingOptions]', N'U') IS NULL
    THROW 50002, N'One or more required tables do not exist.', 1;
GO
IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE [object_id] = OBJECT_ID(N'[meeting_room_anastasia].[RoomOptions]')
      AND [name] = N'IX_MeetingRoom_RoomOptions_RoomId'
)
BEGIN
    CREATE INDEX [IX_MeetingRoom_RoomOptions_RoomId]
        ON [meeting_room_anastasia].[RoomOptions] ([RoomId]);
END;
IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE [object_id] = OBJECT_ID(N'[meeting_room_anastasia].[RoomOptions]')
      AND [name] = N'UX_MeetingRoom_RoomOptions_RoomId_Name_Active'
)
BEGIN
    CREATE UNIQUE INDEX [UX_MeetingRoom_RoomOptions_RoomId_Name_Active]
        ON [meeting_room_anastasia].[RoomOptions] ([RoomId],[Name])
        WHERE [IsActive] = 1;
END;
GO
IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE [object_id] = OBJECT_ID(N'[meeting_room_anastasia].[RoomBookings]')
      AND [name] = N'IX_MeetingRoom_RoomBookings_ConflictSearch'
)
BEGIN
    CREATE INDEX [IX_MeetingRoom_RoomBookings_ConflictSearch]
        ON [meeting_room_anastasia].[RoomBookings] ([RoomId],[Status],[StartTime],[EndTime]);
END;
IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE [object_id] = OBJECT_ID(N'[meeting_room_anastasia].[RoomBookings]')
      AND [name] = N'IX_MeetingRoom_RoomBookings_ReportPeriod'
)
BEGIN
    CREATE INDEX [IX_MeetingRoom_RoomBookings_ReportPeriod]
        ON [meeting_room_anastasia].[RoomBookings] ([Status],[StartTime],[EndTime])
        INCLUDE ([RoomId],[RoomPrice],[OptionsPrice],[TotalPrice]);
END;
GO
IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE [object_id] = OBJECT_ID(N'[meeting_room_anastasia].[BookingOptions]')
      AND [name] = N'IX_MeetingRoom_BookingOptions_RoomBookingId'
)
BEGIN
    CREATE INDEX [IX_MeetingRoom_BookingOptions_RoomBookingId]
        ON [meeting_room_anastasia].[BookingOptions] ([RoomBookingId]);
END;
IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE [object_id] = OBJECT_ID(N'[meeting_room_anastasia].[BookingOptions]')
      AND [name] = N'IX_MeetingRoom_BookingOptions_RoomOptionId'
)
BEGIN
    CREATE INDEX [IX_MeetingRoom_BookingOptions_RoomOptionId]
        ON [meeting_room_anastasia].[BookingOptions] ([RoomOptionId]);
END;
GO
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
GO
IF TYPE_ID(N'meeting_room_anastasia.GuidList') IS NULL
    EXEC(N'CREATE TYPE [meeting_room_anastasia].[GuidList] AS TABLE
    ([Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY);');
GO
IF TYPE_ID(N'meeting_room_anastasia.RoomOptionInput') IS NULL
    EXEC(N'CREATE TYPE [meeting_room_anastasia].[RoomOptionInput] AS TABLE
    ([Name] NVARCHAR(100) NOT NULL,
     [Price] DECIMAL(18,2) NOT NULL,
     CHECK (LEN(LTRIM(RTRIM([Name]))) > 0),
     CHECK ([Price] >= 0),
     UNIQUE ([Name]));');
GO
CREATE OR ALTER FUNCTION [meeting_room_anastasia].[fn_CalculateRoomPrice]
(
    @PricePerHour DECIMAL(18,2),
    @StartTime DATETIME2(7),
    @EndTime DATETIME2(7)
)
RETURNS DECIMAL(18,2)
AS
BEGIN
    DECLARE @Total DECIMAL(38,10) = 0;
    DECLARE @Current DATETIME2(7) = @StartTime;
    DECLARE @Next DATETIME2(7);
    DECLARE @CurrentDate DATE;
    DECLARE @CurrentClock TIME(7);
    DECLARE @Multiplier DECIMAL(5,2);
    DECLARE @Hours DECIMAL(38,10);
    IF @PricePerHour <= 0 OR @StartTime >= @EndTime RETURN NULL;
    WHILE @Current < @EndTime
    BEGIN
        SET @CurrentDate = CAST(@Current AS DATE);
        SET @CurrentClock = CAST(@Current AS TIME(7));
        SET @Next = CASE
            WHEN @CurrentClock < '06:00' THEN DATEADD(HOUR,6,CAST(@CurrentDate AS DATETIME2))
            WHEN @CurrentClock < '09:00' THEN DATEADD(HOUR,9,CAST(@CurrentDate AS DATETIME2))
            WHEN @CurrentClock < '12:00' THEN DATEADD(HOUR,12,CAST(@CurrentDate AS DATETIME2))
            WHEN @CurrentClock < '14:00' THEN DATEADD(HOUR,14,CAST(@CurrentDate AS DATETIME2))
            WHEN @CurrentClock < '18:00' THEN DATEADD(HOUR,18,CAST(@CurrentDate AS DATETIME2))
            WHEN @CurrentClock < '23:00' THEN DATEADD(HOUR,23,CAST(@CurrentDate AS DATETIME2))
            ELSE DATEADD(HOUR,6,CAST(DATEADD(DAY,1,@CurrentDate) AS DATETIME2)) END;
        IF @Next > @EndTime SET @Next = @EndTime;
        SET @Multiplier = CASE
            WHEN @CurrentClock >= '12:00' AND @CurrentClock < '14:00' THEN 1.15
            WHEN @CurrentClock >= '06:00' AND @CurrentClock < '09:00' THEN 0.90
            WHEN @CurrentClock >= '18:00' AND @CurrentClock < '23:00' THEN 0.80
            ELSE 1.00 END;
        SET @Hours = CONVERT(DECIMAL(38,10),DATEDIFF_BIG(MICROSECOND,@Current,@Next))/3600000000.0;
        SET @Total += @PricePerHour*@Multiplier*@Hours;
        SET @Current = @Next;
    END;
    RETURN CAST(ROUND(@Total,2) AS DECIMAL(18,2));
END;
GO
CREATE OR ALTER TRIGGER [meeting_room_anastasia].[TR_MeetingRoom_RoomBookings_PreventOverlap]
ON [meeting_room_anastasia].[RoomBookings]
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    IF EXISTS
    (
        SELECT 1
        FROM inserted n
        JOIN [meeting_room_anastasia].[RoomBookings] e
          WITH (UPDLOCK,HOLDLOCK,INDEX([IX_MeetingRoom_RoomBookings_ConflictSearch]))
          ON e.RoomId=n.RoomId AND e.Status=1 AND n.Status=1 AND e.Id<>n.Id
         AND e.StartTime<n.EndTime AND n.StartTime<e.EndTime
    )
        THROW 50010,N'Meeting room is already booked for this time.',1;
END;
GO
CREATE OR ALTER TRIGGER [meeting_room_anastasia].[TR_MeetingRoom_BookingOptions_ValidateRoom]
ON [meeting_room_anastasia].[BookingOptions]
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    IF EXISTS
    (
        SELECT 1
        FROM inserted i
        JOIN [meeting_room_anastasia].[RoomBookings] b ON b.Id=i.RoomBookingId
        JOIN [meeting_room_anastasia].[RoomOptions] o ON o.Id=i.RoomOptionId
        WHERE o.RoomId<>b.RoomId
    )
        THROW 50011,N'Selected option does not belong to the booked room.',1;
END;
GO
CREATE OR ALTER TRIGGER [meeting_room_anastasia].[TR_MeetingRoom_BookingOptions_RequireActive]
ON [meeting_room_anastasia].[BookingOptions]
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    IF EXISTS
    (
        SELECT 1
        FROM inserted i
        JOIN [meeting_room_anastasia].[RoomOptions] o ON o.Id=i.RoomOptionId
        WHERE o.IsActive=0
    )
        THROW 50012,N'Inactive room option cannot be added to a booking.',1;
END;
GO
CREATE OR ALTER PROCEDURE [meeting_room_anastasia].[usp_Rooms_GetAll]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT r.Id,r.Name,r.Capacity,r.PricePerHour,r.CreatedAt,
           o.Id AS OptionId,o.Name AS OptionName,o.Price AS OptionPrice
    FROM [meeting_room_anastasia].[Rooms] r
    LEFT JOIN [meeting_room_anastasia].[RoomOptions] o ON o.RoomId=r.Id AND o.IsActive=1
    WHERE r.IsDeleted=0 ORDER BY r.Name,o.Name;
END;
GO
CREATE OR ALTER PROCEDURE [meeting_room_anastasia].[usp_Rooms_GetById]
    @RoomId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    SELECT r.Id,r.Name,r.Capacity,r.PricePerHour,r.CreatedAt,
           o.Id AS OptionId,o.Name AS OptionName,o.Price AS OptionPrice
    FROM [meeting_room_anastasia].[Rooms] r
    LEFT JOIN [meeting_room_anastasia].[RoomOptions] o ON o.RoomId=r.Id AND o.IsActive=1
    WHERE r.Id=@RoomId AND r.IsDeleted=0 ORDER BY o.Name;
END;
GO
CREATE OR ALTER PROCEDURE [meeting_room_anastasia].[usp_Rooms_GetAvailable]
    @StartTime DATETIME2(7), @EndTime DATETIME2(7), @Capacity INT
AS
BEGIN
    SET NOCOUNT ON;
    IF @StartTime>=@EndTime THROW 50101,N'Start time must be earlier than end time.',1;
    IF @Capacity<=0 THROW 50102,N'Capacity must be greater than zero.',1;
    SELECT r.Id,r.Name,r.Capacity,r.PricePerHour,r.CreatedAt,
           o.Id AS OptionId,o.Name AS OptionName,o.Price AS OptionPrice
    FROM [meeting_room_anastasia].[Rooms] r
    LEFT JOIN [meeting_room_anastasia].[RoomOptions] o ON o.RoomId=r.Id AND o.IsActive=1
    WHERE r.IsDeleted=0 AND r.Capacity>=@Capacity
      AND NOT EXISTS (SELECT 1 FROM [meeting_room_anastasia].[RoomBookings] b
                      WHERE b.RoomId=r.Id AND b.Status=1
                        AND b.StartTime<@EndTime AND @StartTime<b.EndTime)
    ORDER BY r.Name,o.Name;
END;
GO
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
GO
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
GO
CREATE OR ALTER PROCEDURE [meeting_room_anastasia].[usp_Rooms_SoftDelete]
    @RoomId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE [meeting_room_anastasia].[Rooms] SET IsDeleted=1 WHERE Id=@RoomId AND IsDeleted=0;
    SELECT @@ROWCOUNT AS AffectedRows;
END;
GO
CREATE OR ALTER PROCEDURE [meeting_room_anastasia].[usp_RoomBookings_GetAll]
    @From DATETIME2(7) = NULL,
    @To DATETIME2(7) = NULL,
    @Status INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    IF (@From IS NULL AND @To IS NOT NULL) OR (@From IS NOT NULL AND @To IS NULL)
        THROW 50206,N'Both period boundaries are required.',1;
    IF @From IS NOT NULL AND @From >= @To
        THROW 50207,N'From date must be earlier than to date.',1;
    IF @Status IS NOT NULL AND @Status NOT IN (1,2)
        THROW 50208,N'Unknown booking status.',1;
    SELECT b.Id,b.RoomId,r.Name AS RoomName,b.StartTime,b.EndTime,b.RoomPrice,b.OptionsPrice,b.TotalPrice,b.Status,b.CreatedAt,
           bo.Id AS BookingOptionId,bo.RoomOptionId,bo.OptionName,bo.OptionPrice
    FROM [meeting_room_anastasia].[RoomBookings] b
    JOIN [meeting_room_anastasia].[Rooms] r ON r.Id=b.RoomId
    LEFT JOIN [meeting_room_anastasia].[BookingOptions] bo ON bo.RoomBookingId=b.Id
    WHERE (@From IS NULL OR (b.StartTime<@To AND b.EndTime>@From))
      AND (@Status IS NULL OR b.Status=@Status)
    ORDER BY b.CreatedAt DESC,bo.OptionName;
END;
GO
CREATE OR ALTER PROCEDURE [meeting_room_anastasia].[usp_RoomBookings_GetById]
    @BookingId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    SELECT b.Id,b.RoomId,r.Name AS RoomName,b.StartTime,b.EndTime,b.RoomPrice,b.OptionsPrice,b.TotalPrice,b.Status,b.CreatedAt,
           bo.Id AS BookingOptionId,bo.RoomOptionId,bo.OptionName,bo.OptionPrice
    FROM [meeting_room_anastasia].[RoomBookings] b
    JOIN [meeting_room_anastasia].[Rooms] r ON r.Id=b.RoomId
    LEFT JOIN [meeting_room_anastasia].[BookingOptions] bo ON bo.RoomBookingId=b.Id
    WHERE b.Id=@BookingId ORDER BY bo.OptionName;
END;
GO
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
GO
CREATE OR ALTER PROCEDURE [meeting_room_anastasia].[usp_RoomBookings_Cancel]
    @BookingId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE [meeting_room_anastasia].[RoomBookings] SET Status=2 WHERE Id=@BookingId AND Status=1;
    SELECT @@ROWCOUNT AS AffectedRows;
END;
GO
CREATE OR ALTER PROCEDURE [meeting_room_anastasia].[usp_Reports_GetRevenue]
    @From DATETIME2(7), @To DATETIME2(7)
AS
BEGIN
    SET NOCOUNT ON;
    IF @From>=@To THROW 50301,N'From must be earlier than To.',1;
    SELECT COUNT_BIG(*) AS BookingsCount,COALESCE(SUM(RoomPrice),0) AS RoomRevenue,
           COALESCE(SUM(OptionsPrice),0) AS OptionsRevenue,COALESCE(SUM(TotalPrice),0) AS TotalRevenue
    FROM [meeting_room_anastasia].[RoomBookings]
    WHERE Status=1 AND StartTime<@To AND EndTime>@From;
END;
GO
CREATE OR ALTER PROCEDURE [meeting_room_anastasia].[usp_Reports_GetPopularOptions]
    @From DATETIME2(7), @To DATETIME2(7)
AS
BEGIN
    SET NOCOUNT ON;
    IF @From>=@To THROW 50302,N'From must be earlier than To.',1;
    SELECT bo.OptionName,COUNT_BIG(*) AS UsageCount,SUM(bo.OptionPrice) AS Revenue
    FROM [meeting_room_anastasia].[BookingOptions] bo
    JOIN [meeting_room_anastasia].[RoomBookings] b ON b.Id=bo.RoomBookingId
    WHERE b.Status=1 AND b.StartTime<@To AND b.EndTime>@From
    GROUP BY bo.OptionName ORDER BY UsageCount DESC,bo.OptionName;
END;
GO
CREATE OR ALTER PROCEDURE [meeting_room_anastasia].[usp_Reports_GetRoomUsage]
    @From DATETIME2(7), @To DATETIME2(7)
AS
BEGIN
    SET NOCOUNT ON;
    IF @From>=@To THROW 50303,N'From must be earlier than To.',1;
    SELECT r.Name AS RoomName,COUNT_BIG(*) AS BookingsCount,
           SUM(CONVERT(DECIMAL(18,2),DATEDIFF_BIG(SECOND,b.StartTime,b.EndTime))/3600.0) AS BookedHours,
           SUM(b.TotalPrice) AS Revenue
    FROM [meeting_room_anastasia].[RoomBookings] b
    JOIN [meeting_room_anastasia].[Rooms] r ON r.Id=b.RoomId
    WHERE b.Status=1 AND b.StartTime<@To AND b.EndTime>@From
    GROUP BY r.Name ORDER BY BookingsCount DESC,r.Name;
END;
GO
IF NOT EXISTS (SELECT 1 FROM [meeting_room_anastasia].[Rooms] WHERE [Id]='00000000-0000-0000-0000-000000000001')
BEGIN
    INSERT [meeting_room_anastasia].[Rooms] ([Id],[Name],[Capacity],[PricePerHour],[IsDeleted],[CreatedAt]) VALUES
    ('00000000-0000-0000-0000-000000000001',N'Hall A',50,2000,0,'2026-01-01T00:00:00'),
    ('00000000-0000-0000-0000-000000000002',N'Hall B',100,3500,0,'2026-01-01T00:00:00'),
    ('00000000-0000-0000-0000-000000000003',N'Hall C',30,1500,0,'2026-01-01T00:00:00');
END;
IF NOT EXISTS (SELECT 1 FROM [meeting_room_anastasia].[RoomOptions] WHERE [Id]='00000000-0000-0000-0000-000000000101')
BEGIN
    INSERT [meeting_room_anastasia].[RoomOptions] ([Id],[RoomId],[Name],[Price],[IsActive]) VALUES
    ('00000000-0000-0000-0000-000000000101','00000000-0000-0000-0000-000000000001',N'Projector',500,1),
    ('00000000-0000-0000-0000-000000000102','00000000-0000-0000-0000-000000000001',N'Wi-Fi',300,1),
    ('00000000-0000-0000-0000-000000000103','00000000-0000-0000-0000-000000000001',N'Sound',700,1),
    ('00000000-0000-0000-0000-000000000201','00000000-0000-0000-0000-000000000002',N'Projector',500,1),
    ('00000000-0000-0000-0000-000000000202','00000000-0000-0000-0000-000000000002',N'Wi-Fi',300,1),
    ('00000000-0000-0000-0000-000000000203','00000000-0000-0000-0000-000000000002',N'Sound',700,1),
    ('00000000-0000-0000-0000-000000000301','00000000-0000-0000-0000-000000000003',N'Projector',500,1),
    ('00000000-0000-0000-0000-000000000302','00000000-0000-0000-0000-000000000003',N'Wi-Fi',300,1),
    ('00000000-0000-0000-0000-000000000303','00000000-0000-0000-0000-000000000003',N'Sound',700,1);
END;
GO
SET NOCOUNT ON;
SELECT s.name AS SchemaName,t.name AS TableName
FROM sys.tables t JOIN sys.schemas s ON s.schema_id=t.schema_id
WHERE s.name=N'meeting_room_anastasia' ORDER BY t.name;
SELECT t.name AS TableName,i.name AS IndexName,i.is_unique,i.is_disabled
FROM sys.indexes i JOIN sys.tables t ON t.object_id=i.object_id
WHERE OBJECT_SCHEMA_NAME(i.object_id)=N'meeting_room_anastasia' AND i.name IS NOT NULL
ORDER BY t.name,i.name;
SELECT tr.name AS TriggerName,OBJECT_NAME(tr.parent_id) AS TableName,tr.is_disabled
FROM sys.triggers tr
WHERE OBJECT_SCHEMA_NAME(tr.parent_id)=N'meeting_room_anastasia' ORDER BY tr.name;
SELECT o.type_desc,o.name
FROM sys.objects o
WHERE SCHEMA_NAME(o.schema_id)=N'meeting_room_anastasia' AND o.type IN (N'P',N'FN')
ORDER BY o.type_desc,o.name;
SELECT tt.name AS TableTypeName
FROM sys.table_types tt
WHERE SCHEMA_NAME(tt.schema_id)=N'meeting_room_anastasia' ORDER BY tt.name;
SELECT N'Rooms' AS TableName,COUNT_BIG(*) AS RecordsCount FROM [meeting_room_anastasia].[Rooms]
UNION ALL SELECT N'RoomOptions',COUNT_BIG(*) FROM [meeting_room_anastasia].[RoomOptions]
UNION ALL SELECT N'RoomBookings',COUNT_BIG(*) FROM [meeting_room_anastasia].[RoomBookings]
UNION ALL SELECT N'BookingOptions',COUNT_BIG(*) FROM [meeting_room_anastasia].[BookingOptions];
GO
PRINT N'Database deployment completed successfully.';
GO
