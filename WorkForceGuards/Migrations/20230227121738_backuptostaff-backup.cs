using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkForceManagementV0.Migrations
{
    public partial class backuptostaffbackup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BackupToStaffId",
                table: "BkpScheduleDetails",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BkpScheduleDetails_BackupToStaffId",
                table: "BkpScheduleDetails",
                column: "BackupToStaffId");

            migrationBuilder.AddForeignKey(
                name: "FK_BkpScheduleDetails_StaffMembers_BackupToStaffId",
                table: "BkpScheduleDetails",
                column: "BackupToStaffId",
                principalTable: "StaffMembers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BkpScheduleDetails_StaffMembers_BackupToStaffId",
                table: "BkpScheduleDetails");

            migrationBuilder.DropIndex(
                name: "IX_BkpScheduleDetails_BackupToStaffId",
                table: "BkpScheduleDetails");

            migrationBuilder.DropColumn(
                name: "BackupToStaffId",
                table: "BkpScheduleDetails");
        }
    }
}
