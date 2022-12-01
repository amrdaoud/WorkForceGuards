using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkForceManagementV0.Migrations
{
    public partial class sublocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubLocation_Locations_LocationId",
                table: "SubLocation");

            migrationBuilder.DropForeignKey(
                name: "FK_TransportationRoutes_SubLocation_SubLocationId",
                table: "TransportationRoutes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubLocation",
                table: "SubLocation");

            migrationBuilder.RenameTable(
                name: "SubLocation",
                newName: "SubLocations");

            migrationBuilder.RenameIndex(
                name: "IX_SubLocation_LocationId",
                table: "SubLocations",
                newName: "IX_SubLocations_LocationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubLocations",
                table: "SubLocations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubLocations_Locations_LocationId",
                table: "SubLocations",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TransportationRoutes_SubLocations_SubLocationId",
                table: "TransportationRoutes",
                column: "SubLocationId",
                principalTable: "SubLocations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubLocations_Locations_LocationId",
                table: "SubLocations");

            migrationBuilder.DropForeignKey(
                name: "FK_TransportationRoutes_SubLocations_SubLocationId",
                table: "TransportationRoutes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubLocations",
                table: "SubLocations");

            migrationBuilder.RenameTable(
                name: "SubLocations",
                newName: "SubLocation");

            migrationBuilder.RenameIndex(
                name: "IX_SubLocations_LocationId",
                table: "SubLocation",
                newName: "IX_SubLocation_LocationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubLocation",
                table: "SubLocation",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubLocation_Locations_LocationId",
                table: "SubLocation",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TransportationRoutes_SubLocation_SubLocationId",
                table: "TransportationRoutes",
                column: "SubLocationId",
                principalTable: "SubLocation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
