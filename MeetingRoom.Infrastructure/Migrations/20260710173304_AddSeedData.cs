using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MeetingRoom.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Capacity = table.Column<int>(type: "integer", nullable: false),
                    PricePerHour = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoomBookings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RoomId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RoomPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    OptionsPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomBookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoomBookings_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RoomOptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RoomId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoomOptions_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookingOptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RoomBookingId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoomOptionId = table.Column<Guid>(type: "uuid", nullable: false),
                    OptionName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OptionPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingOptions_RoomBookings_RoomBookingId",
                        column: x => x.RoomBookingId,
                        principalTable: "RoomBookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingOptions_RoomOptions_RoomOptionId",
                        column: x => x.RoomOptionId,
                        principalTable: "RoomOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "Capacity", "CreatedAt", "Name", "PricePerHour" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), 50, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Hall A", 2000m },
                    { new Guid("00000000-0000-0000-0000-000000000002"), 100, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Hall B", 3500m },
                    { new Guid("00000000-0000-0000-0000-000000000003"), 30, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Hall C", 1500m }
                });

            migrationBuilder.InsertData(
                table: "RoomOptions",
                columns: new[] { "Id", "IsActive", "Name", "Price", "RoomId" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000101"), true, "Projector", 500m, new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("00000000-0000-0000-0000-000000000102"), true, "Wi-Fi", 300m, new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("00000000-0000-0000-0000-000000000103"), true, "Sound", 700m, new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("00000000-0000-0000-0000-000000000201"), true, "Projector", 500m, new Guid("00000000-0000-0000-0000-000000000002") },
                    { new Guid("00000000-0000-0000-0000-000000000202"), true, "Wi-Fi", 300m, new Guid("00000000-0000-0000-0000-000000000002") },
                    { new Guid("00000000-0000-0000-0000-000000000203"), true, "Sound", 700m, new Guid("00000000-0000-0000-0000-000000000002") },
                    { new Guid("00000000-0000-0000-0000-000000000301"), true, "Projector", 500m, new Guid("00000000-0000-0000-0000-000000000003") },
                    { new Guid("00000000-0000-0000-0000-000000000302"), true, "Wi-Fi", 300m, new Guid("00000000-0000-0000-0000-000000000003") },
                    { new Guid("00000000-0000-0000-0000-000000000303"), true, "Sound", 700m, new Guid("00000000-0000-0000-0000-000000000003") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingOptions_RoomBookingId",
                table: "BookingOptions",
                column: "RoomBookingId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingOptions_RoomOptionId",
                table: "BookingOptions",
                column: "RoomOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomBookings_RoomId",
                table: "RoomBookings",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomOptions_RoomId",
                table: "RoomOptions",
                column: "RoomId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingOptions");

            migrationBuilder.DropTable(
                name: "RoomBookings");

            migrationBuilder.DropTable(
                name: "RoomOptions");

            migrationBuilder.DropTable(
                name: "Rooms");
        }
    }
}
