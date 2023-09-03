using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkForceManagementV0.Migrations
{
    public partial class backuptostaff : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BackupToStaffId",
                table: "ScheduleDetail",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleDetail_BackupToStaffId",
                table: "ScheduleDetail",
                column: "BackupToStaffId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduleDetail_StaffMembers_BackupToStaffId",
                table: "ScheduleDetail",
                column: "BackupToStaffId",
                principalTable: "StaffMembers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScheduleDetail_StaffMembers_BackupToStaffId",
                table: "ScheduleDetail");

            migrationBuilder.DropIndex(
                name: "IX_ScheduleDetail_BackupToStaffId",
                table: "ScheduleDetail");

            migrationBuilder.DropColumn(
                name: "BackupToStaffId",
                table: "ScheduleDetail");
        }
    }
}
