IF TYPE_ID(N'meeting_room_anastasia.RoomOptionInput') IS NULL
    EXEC(N'CREATE TYPE [meeting_room_anastasia].[RoomOptionInput] AS TABLE
    ([Name] NVARCHAR(100) NOT NULL,
     [Price] DECIMAL(18,2) NOT NULL,
     CHECK (LEN(LTRIM(RTRIM([Name]))) > 0),
     CHECK ([Price] >= 0),
     UNIQUE ([Name]));');
