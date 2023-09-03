using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkForceManagementV0.Migrations
{
    public partial class headcount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Headcounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransportationRouteId = table.Column<int>(type: "int", nullable: false),
                    SublocationId = table.Column<int>(type: "int", nullable: false),
                    Total = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Headcounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Headcounts_SubLocations_SublocationId",
                        column: x => x.SublocationId,
                        principalTable: "SubLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Headcounts_TransportationRoutes_TransportationRouteId",
                        column: x => x.TransportationRouteId,
                        principalTable: "TransportationRoutes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Headcounts_SublocationId",
                table: "Headcounts",
                column: "SublocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Headcounts_TransportationRouteId",
                table: "Headcounts",
                column: "TransportationRouteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Headcounts");
        }
    }
}
