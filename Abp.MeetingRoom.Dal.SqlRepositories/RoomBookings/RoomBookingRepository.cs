using System.Data;
using Abp.MeetingRoom.Bll.Common.RoomBookings;
using Abp.MeetingRoom.Bll.Common.RoomBookings.Models;
using Abp.MeetingRoom.Dal.SqlRepositories.Database;
using Abp.MeetingRoom.Dal.SqlRepositories.RoomBookings.Formatters;
using Abp.MeetingRoom.Dal.SqlRepositories.RoomBookings.Mappings;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
namespace Abp.MeetingRoom.Dal.SqlRepositories.RoomBookings;
internal sealed class RoomBookingRepository : IRoomBookingRepository
{
    private readonly string _connectionString;
    public RoomBookingRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Connection string 'DefaultConnection' is not configured."
            );
    }
    public Task<List<RoomBooking>> GetAllAsync() => QueryBookingsAsync();
    public async Task<RoomBooking?> GetByIdAsync(Guid id)
    {
        var bookings = await QueryBookingsAsync(id);
        return bookings.SingleOrDefault();
    }
    public async Task<RoomBooking> CreateAsync(
        Guid roomId,
        DateTime startTime,
        DateTime endTime,
        IReadOnlyCollection<Guid> selectedOptionIds
    )
    {
        return await SqlOperationExecutor.ExecuteAsync(async () =>
        {
            await using var connection = new SqlConnection(_connectionString);
            await using var command = CreateCommand(
                connection,
                SqlObjectNames.RoomBookings.Create
            );
            command.Parameters.Add("@RoomId", SqlDbType.UniqueIdentifier).Value = roomId;
            AddDateTime2Parameter(command, "@StartTime", startTime);
            AddDateTime2Parameter(command, "@EndTime", endTime);
            AddSelectedOptionIds(command, selectedOptionIds);
            await connection.OpenAsync();
            await using var reader = await command.ExecuteReaderAsync();
            return (await RoomBookingDataMapper.ReadAsync(reader)).Single();
        });
    }
    public async Task<bool> CancelAsync(Guid id)
    {
        return await SqlOperationExecutor.ExecuteAsync(async () =>
        {
            await using var connection = new SqlConnection(_connectionString);
            await using var command = CreateCommand(
                connection,
                SqlObjectNames.RoomBookings.Cancel
            );
            command.Parameters.Add("@BookingId", SqlDbType.UniqueIdentifier).Value = id;
            await connection.OpenAsync();
            return Convert.ToInt32(await command.ExecuteScalarAsync()) == 1;
        });
    }
    public Task<List<RoomBooking>> GetByPeriodAsync(DateTime from, DateTime to)
    {
        return QueryBookingsAsync(from: from, to: to);
    }
    public Task<List<RoomBooking>> GetActiveByPeriodAsync(DateTime from, DateTime to)
    {
        return QueryBookingsAsync(from: from, to: to, status: BookingStatus.Active);
    }
    private async Task<List<RoomBooking>> QueryBookingsAsync(
        Guid? id = null,
        DateTime? from = null,
        DateTime? to = null,
        BookingStatus? status = null
    )
    {
        return await SqlOperationExecutor.ExecuteAsync(async () =>
        {
            await using var connection = new SqlConnection(_connectionString);
            var procedure = id.HasValue
                ? SqlObjectNames.RoomBookings.GetById
                : SqlObjectNames.RoomBookings.GetAll;
            await using var command = CreateCommand(connection, procedure);
            if (id.HasValue)
            {
                command.Parameters.Add("@BookingId", SqlDbType.UniqueIdentifier).Value =
                    id.Value;
            }
            else
            {
                AddDateTime2Parameter(command, "@From", from);
                AddDateTime2Parameter(command, "@To", to);
                command.Parameters.Add("@Status", SqlDbType.Int).Value =
                    status.HasValue ? (object)(int)status.Value : DBNull.Value;
            }
            await connection.OpenAsync();
            await using var reader = await command.ExecuteReaderAsync();
            return await RoomBookingDataMapper.ReadAsync(reader);
        });
    }
    private static SqlCommand CreateCommand(SqlConnection connection, string procedure)
    {
        return new SqlCommand(procedure, connection)
        {
            CommandType = CommandType.StoredProcedure,
        };
    }
    private static void AddSelectedOptionIds(SqlCommand command, IEnumerable<Guid> ids)
    {
        var parameter = command.Parameters.Add("@SelectedOptionIds", SqlDbType.Structured);
        parameter.TypeName = SqlObjectNames.Types.GuidList;
        parameter.Value = GuidListTableFormatter.Create(ids);
    }
    private static void AddDateTime2Parameter(
        SqlCommand command,
        string name,
        DateTime? value
    )
    {
        var parameter = command.Parameters.Add(name, SqlDbType.DateTime2);
        parameter.Scale = 7;
        parameter.Value = value.HasValue ? value.Value : DBNull.Value;
    }
}
