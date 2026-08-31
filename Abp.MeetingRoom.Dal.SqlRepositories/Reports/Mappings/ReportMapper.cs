using Abp.MeetingRoom.Bll.Common.Reports.Models;
using Abp.MeetingRoom.Dal.SqlRepositories.Reports.Entities;
using Microsoft.Data.SqlClient;
namespace Abp.MeetingRoom.Dal.SqlRepositories.Reports.Mappings;
internal static class ReportMapper
{
    public static RevenueReport ReadRevenue(SqlDataReader reader)
    {
        var entity = new RevenueReportEntity
        {
            BookingsCount = checked((int)reader.GetInt64(reader.GetOrdinal("BookingsCount"))),
            RoomRevenue = reader.GetDecimal(reader.GetOrdinal("RoomRevenue")),
            OptionsRevenue = reader.GetDecimal(reader.GetOrdinal("OptionsRevenue")),
            TotalRevenue = reader.GetDecimal(reader.GetOrdinal("TotalRevenue")),
        };
        return new RevenueReport
        {
            BookingsCount = entity.BookingsCount,
            RoomRevenue = entity.RoomRevenue,
            OptionsRevenue = entity.OptionsRevenue,
            TotalRevenue = entity.TotalRevenue,
        };
    }
    public static async Task<List<PopularOptionReport>> ReadPopularOptionsAsync(
        SqlDataReader reader,
        CancellationToken cancellationToken
    )
    {
        var entities = new List<PopularOptionReportEntity>();
        while (await reader.ReadAsync(cancellationToken))
        {
            entities.Add(new PopularOptionReportEntity
            {
                OptionName = reader.GetString(reader.GetOrdinal("OptionName")),
                UsageCount = checked((int)reader.GetInt64(reader.GetOrdinal("UsageCount"))),
                Revenue = reader.GetDecimal(reader.GetOrdinal("Revenue")),
            });
        }
        return entities
            .Select(entity => new PopularOptionReport
            {
                OptionName = entity.OptionName,
                UsageCount = entity.UsageCount,
                Revenue = entity.Revenue,
            })
            .ToList();
    }
    public static async Task<List<RoomUsageReport>> ReadRoomUsageAsync(
        SqlDataReader reader,
        CancellationToken cancellationToken
    )
    {
        var entities = new List<RoomUsageReportEntity>();
        while (await reader.ReadAsync(cancellationToken))
        {
            entities.Add(new RoomUsageReportEntity
            {
                RoomName = reader.GetString(reader.GetOrdinal("RoomName")),
                BookingsCount = checked((int)reader.GetInt64(reader.GetOrdinal("BookingsCount"))),
                BookedHours = Convert.ToDouble(reader.GetDecimal(reader.GetOrdinal("BookedHours"))),
                Revenue = reader.GetDecimal(reader.GetOrdinal("Revenue")),
            });
        }
        return entities
            .Select(entity => new RoomUsageReport
            {
                RoomName = entity.RoomName,
                BookingsCount = entity.BookingsCount,
                BookedHours = entity.BookedHours,
                Revenue = entity.Revenue,
            })
            .ToList();
    }
}
