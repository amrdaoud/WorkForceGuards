using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkForceManagementV0.Migrations
{
    public partial class addbackupandsublocationtobackups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BackupStaffId",
                table: "BkpScheduleDetails",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SublocationId",
                table: "BkpDailyAttendances",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BkpScheduleDetails_BackupStaffId",
                table: "BkpScheduleDetails",
                column: "BackupStaffId");

            migrationBuilder.AddForeignKey(
                name: "FK_BkpScheduleDetails_StaffMembers_BackupStaffId",
                table: "BkpScheduleDetails",
                column: "BackupStaffId",
                principalTable: "StaffMembers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BkpScheduleDetails_StaffMembers_BackupStaffId",
                table: "BkpScheduleDetails");

            migrationBuilder.DropIndex(
                name: "IX_BkpScheduleDetails_BackupStaffId",
                table: "BkpScheduleDetails");

            migrationBuilder.DropColumn(
                name: "BackupStaffId",
                table: "BkpScheduleDetails");

            migrationBuilder.DropColumn(
                name: "SublocationId",
                table: "BkpDailyAttendances");
        }
    }
}
