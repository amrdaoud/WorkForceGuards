using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkForceManagementV0.Migrations
{
    public partial class justification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Justification",
                table: "ScheduleDetail",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Justification",
                table: "ScheduleDetail");
        }
    }
}
