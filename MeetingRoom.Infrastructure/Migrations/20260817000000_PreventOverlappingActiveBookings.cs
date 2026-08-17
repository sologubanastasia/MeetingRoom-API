using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeetingRoom.Infrastructure.Migrations;

/// <summary>
/// Prevents concurrent requests from creating overlapping active bookings
/// for the same conference room.
/// </summary>
[DbContext(typeof(RoomDbContext))]
[Migration("20260817000000_PreventOverlappingActiveBookings")]
public partial class PreventOverlappingActiveBookings : Migration
{
    /// <summary>
    /// Adds a PostgreSQL exclusion constraint for active booking time ranges.
    /// </summary>
    /// <param name="migrationBuilder">Builder used to define migration operations.</param>
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("CREATE EXTENSION IF NOT EXISTS btree_gist;");

        migrationBuilder.Sql(
            """
            ALTER TABLE "RoomBookings"
            ADD CONSTRAINT "EX_RoomBookings_NoOverlappingActiveBookings"
            EXCLUDE USING gist
            (
                "RoomId" WITH =,
                tstzrange("StartTime", "EndTime", '[)') WITH &&
            )
            WHERE ("Status" = 1);
            """
        );
    }

    /// <summary>
    /// Removes the exclusion constraint.
    /// </summary>
    /// <param name="migrationBuilder">Builder used to define migration operations.</param>
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(
            """
            ALTER TABLE "RoomBookings"
            DROP CONSTRAINT "EX_RoomBookings_NoOverlappingActiveBookings";
            """
        );
    }
}
