IF TYPE_ID(N'meeting_room_anastasia.GuidList') IS NULL
    EXEC(N'CREATE TYPE [meeting_room_anastasia].[GuidList] AS TABLE
    ([Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY);');
