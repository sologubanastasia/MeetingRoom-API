using System.Data;
using Abp.MeetingRoom.Bll.Common.Rooms.Models;
namespace Abp.MeetingRoom.Dal.SqlRepositories.Rooms.Formatters;
internal static class RoomOptionTableFormatter
{
    public static DataTable Create(IEnumerable<RoomOption> options)
    {
        var table = new DataTable();
        table.Columns.Add("Name", typeof(string));
        table.Columns.Add("Price", typeof(decimal));
        foreach (var option in options)
        {
            table.Rows.Add(option.Name, option.Price);
        }
        return table;
    }
}
