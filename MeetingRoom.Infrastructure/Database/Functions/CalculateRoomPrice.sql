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
