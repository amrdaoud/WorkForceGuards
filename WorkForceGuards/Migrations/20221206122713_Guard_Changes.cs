using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkForceManagementV0.Migrations
{
    public partial class Guard_Changes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SublocationId",
                table: "DailyAttendances",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SublocationId",
                table: "breakTypeOptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "DailyAttendancePatterns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScheduleId = table.Column<int>(type: "int", nullable: false),
                    StaffMemberId = table.Column<int>(type: "int", nullable: false),
                    SublocationId = table.Column<int>(type: "int", nullable: false),
                    TransportationId = table.Column<int>(type: "int", nullable: false),
                    DayOffs = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyAttendancePatterns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyAttendancePatterns_Schedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DailyAttendancePatterns_StaffMembers_StaffMemberId",
                        column: x => x.StaffMemberId,
                        principalTable: "StaffMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DailyAttendancePatterns_SubLocations_SublocationId",
                        column: x => x.SublocationId,
                        principalTable: "SubLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DailyAttendancePatterns_TransportationRoutes_TransportationId",
                        column: x => x.TransportationId,
                        principalTable: "TransportationRoutes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DailyAttendances_SublocationId",
                table: "DailyAttendances",
                column: "SublocationId");

            migrationBuilder.CreateIndex(
                name: "IX_breakTypeOptions_SublocationId",
                table: "breakTypeOptions",
                column: "SublocationId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyAttendancePatterns_ScheduleId",
                table: "DailyAttendancePatterns",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyAttendancePatterns_StaffMemberId",
                table: "DailyAttendancePatterns",
                column: "StaffMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyAttendancePatterns_SublocationId",
                table: "DailyAttendancePatterns",
                column: "SublocationId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyAttendancePatterns_TransportationId",
                table: "DailyAttendancePatterns",
                column: "TransportationId");

            migrationBuilder.AddForeignKey(
                name: "FK_breakTypeOptions_SubLocations_SublocationId",
                table: "breakTypeOptions",
                column: "SublocationId",
                principalTable: "SubLocations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DailyAttendances_SubLocations_SublocationId",
                table: "DailyAttendances",
                column: "SublocationId",
                principalTable: "SubLocations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_breakTypeOptions_SubLocations_SublocationId",
                table: "breakTypeOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_DailyAttendances_SubLocations_SublocationId",
                table: "DailyAttendances");

            migrationBuilder.DropTable(
                name: "DailyAttendancePatterns");

            migrationBuilder.DropIndex(
                name: "IX_DailyAttendances_SublocationId",
                table: "DailyAttendances");

            migrationBuilder.DropIndex(
                name: "IX_breakTypeOptions_SublocationId",
                table: "breakTypeOptions");

            migrationBuilder.DropColumn(
                name: "SublocationId",
                table: "DailyAttendances");

            migrationBuilder.DropColumn(
                name: "SublocationId",
                table: "breakTypeOptions");
        }
    }
}
