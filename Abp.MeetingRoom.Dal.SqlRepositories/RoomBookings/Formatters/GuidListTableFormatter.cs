using System.Data;
namespace Abp.MeetingRoom.Dal.SqlRepositories.RoomBookings.Formatters;
internal static class GuidListTableFormatter
{
    public static DataTable Create(IEnumerable<Guid> ids)
    {
        var table = new DataTable();
        table.Columns.Add("Id", typeof(Guid));
        foreach (var id in ids)
        {
            table.Rows.Add(id);
        }
        return table;
    }
}
