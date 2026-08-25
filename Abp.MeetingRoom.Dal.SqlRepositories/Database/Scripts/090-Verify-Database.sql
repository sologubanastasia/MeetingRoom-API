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
