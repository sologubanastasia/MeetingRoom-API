using System.Data;
using Abp.MeetingRoom.Bll.Common.Rooms;
using Abp.MeetingRoom.Bll.Common.Rooms.Models;
using Abp.MeetingRoom.Dal.SqlRepositories.Database;
using Abp.MeetingRoom.Dal.SqlRepositories.Rooms.Formatters;
using Abp.MeetingRoom.Dal.SqlRepositories.Rooms.Mappings;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
namespace Abp.MeetingRoom.Dal.SqlRepositories.Rooms;
internal sealed class RoomRepository : IRoomRepository
{
    private readonly string _connectionString;
    public RoomRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Connection string 'DefaultConnection' is not configured."
            );
    }
    public Task<List<Room>> GetAllAsync() => QueryRoomsAsync(SqlObjectNames.Rooms.GetAll);
    public async Task<Room?> GetByIdAsync(Guid id)
    {
        var rooms = await QueryRoomsAsync(SqlObjectNames.Rooms.GetById, command =>
            command.Parameters.Add("@RoomId", SqlDbType.UniqueIdentifier).Value = id);
        return rooms.SingleOrDefault();
    }
    public Task<List<Room>> GetAvailableAsync(
        DateTime startTime,
        DateTime endTime,
        int capacity
    )
    {
        return QueryRoomsAsync(SqlObjectNames.Rooms.GetAvailable, command =>
        {
            command.Parameters.Add("@StartTime", SqlDbType.DateTime2).Value = startTime;
            command.Parameters.Add("@EndTime", SqlDbType.DateTime2).Value = endTime;
            command.Parameters.Add("@Capacity", SqlDbType.Int).Value = capacity;
        });
    }
    public async Task<Room> CreateAsync(Room room)
    {
        var rooms = await QueryRoomsAsync(SqlObjectNames.Rooms.Create, command =>
        {
            AddRoomValues(command, room);
            AddOptions(command, room.Options);
        });
        return rooms.Single();
    }
    public async Task<Room> UpdateAsync(Room room)
    {
        var rooms = await QueryRoomsAsync(SqlObjectNames.Rooms.Update, command =>
        {
            command.Parameters.Add("@RoomId", SqlDbType.UniqueIdentifier).Value = room.Id;
            AddRoomValues(command, room);
            AddOptions(command, room.Options.Where(option => option.IsActive));
        });
        return rooms.Single();
    }
    public async Task<bool> SoftDeleteAsync(Guid id)
    {
        return await SqlOperationExecutor.ExecuteAsync(async () =>
        {
            await using var connection = new SqlConnection(_connectionString);
            await using var command = CreateCommand(
                connection,
                SqlObjectNames.Rooms.SoftDelete
            );
            command.Parameters.Add("@RoomId", SqlDbType.UniqueIdentifier).Value = id;
            await connection.OpenAsync();
            return Convert.ToInt32(await command.ExecuteScalarAsync()) == 1;
        });
    }
    private async Task<List<Room>> QueryRoomsAsync(
        string procedure,
        Action<SqlCommand>? configure = null
    )
    {
        return await SqlOperationExecutor.ExecuteAsync(async () =>
        {
            await using var connection = new SqlConnection(_connectionString);
            await using var command = CreateCommand(connection, procedure);
            configure?.Invoke(command);
            await connection.OpenAsync();
            await using var reader = await command.ExecuteReaderAsync();
            return await RoomDataMapper.ReadAsync(reader);
        });
    }
    private static SqlCommand CreateCommand(SqlConnection connection, string procedure)
    {
        return new SqlCommand(procedure, connection)
        {
            CommandType = CommandType.StoredProcedure,
        };
    }
    private static void AddRoomValues(SqlCommand command, Room room)
    {
        command.Parameters.Add("@Name", SqlDbType.NVarChar, 100).Value = room.Name;
        command.Parameters.Add("@Capacity", SqlDbType.Int).Value = room.Capacity;
        var price = command.Parameters.Add("@PricePerHour", SqlDbType.Decimal);
        price.Precision = 18;
        price.Scale = 2;
        price.Value = room.PricePerHour;
    }
    private static void AddOptions(SqlCommand command, IEnumerable<RoomOption> options)
    {
        var parameter = command.Parameters.AddWithValue(
            "@Options",
            RoomOptionTableFormatter.Create(options)
        );
        parameter.SqlDbType = SqlDbType.Structured;
        parameter.TypeName = SqlObjectNames.Types.RoomOptionInput;
    }
}
