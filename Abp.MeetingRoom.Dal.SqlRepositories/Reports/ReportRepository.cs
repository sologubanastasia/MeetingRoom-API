using System.Data;
using Abp.MeetingRoom.Bll.Common.Reports;
using Abp.MeetingRoom.Bll.Common.Reports.Models;
using Abp.MeetingRoom.Dal.SqlRepositories.Database;
using Abp.MeetingRoom.Dal.SqlRepositories.Database.Exceptions;
using Abp.MeetingRoom.Dal.SqlRepositories.Reports.Mappings;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
namespace Abp.MeetingRoom.Dal.SqlRepositories.Reports;
internal sealed class ReportRepository : IReportRepository
{
    private readonly string _connectionString;
    public ReportRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Connection string 'DefaultConnection' is not configured."
            );
    }
    public async Task<RevenueReport> GetRevenueAsync(
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken
    )
    {
        return await SqlOperationExecutor.ExecuteAsync(async () =>
        {
            await using var connection = new SqlConnection(_connectionString);
            await using var command = CreateReportCommand(
                connection,
                SqlObjectNames.Reports.GetRevenue,
                from,
                to
            );
            await connection.OpenAsync(cancellationToken);
            await using var reader = await command.ExecuteReaderAsync(
                CommandBehavior.SingleRow,
                cancellationToken
            );
            if (!await reader.ReadAsync(cancellationToken))
            {
                throw new DatabaseOperationException(
                    "A database error occurred.",
                    "The revenue report procedure did not return a result."
                );
            }
            return ReportMapper.ReadRevenue(reader);
        });
    }
    public async Task<List<PopularOptionReport>> GetPopularOptionsAsync(
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken
    )
    {
        return await SqlOperationExecutor.ExecuteAsync(async () =>
        {
            await using var connection = new SqlConnection(_connectionString);
            await using var command = CreateReportCommand(
                connection,
                SqlObjectNames.Reports.GetPopularOptions,
                from,
                to
            );
            await connection.OpenAsync(cancellationToken);
            await using var reader = await command.ExecuteReaderAsync(cancellationToken);
            return await ReportMapper.ReadPopularOptionsAsync(reader, cancellationToken);
        });
    }
    public async Task<List<RoomUsageReport>> GetRoomUsageAsync(
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken
    )
    {
        return await SqlOperationExecutor.ExecuteAsync(async () =>
        {
            await using var connection = new SqlConnection(_connectionString);
            await using var command = CreateReportCommand(
                connection,
                SqlObjectNames.Reports.GetRoomUsage,
                from,
                to
            );
            await connection.OpenAsync(cancellationToken);
            await using var reader = await command.ExecuteReaderAsync(cancellationToken);
            return await ReportMapper.ReadRoomUsageAsync(reader, cancellationToken);
        });
    }
    private static SqlCommand CreateReportCommand(
        SqlConnection connection,
        string procedure,
        DateTime from,
        DateTime to
    )
    {
        var command = new SqlCommand(procedure, connection)
        {
            CommandType = CommandType.StoredProcedure,
        };
        var fromParameter = command.Parameters.Add("@From", SqlDbType.DateTime2);
        fromParameter.Scale = 7;
        fromParameter.Value = from;
        var toParameter = command.Parameters.Add("@To", SqlDbType.DateTime2);
        toParameter.Scale = 7;
        toParameter.Value = to;
        return command;
    }
}
