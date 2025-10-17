using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NRLObstacleReporting.Database.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ObstacleCompleteModel",
                columns: table => new
                {
                    IsDraft = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ObstacleId = table.Column<int>(type: "int", nullable: false),
                    ObstacleHeightMeter = table.Column<int>(type: "int", nullable: false),
                    GeometryGeoJson = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ObstacleName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ObstacleDescription = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ObstacleIlluminated = table.Column<bool>(type: "tinyint(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObstacleCompleteModel", x => x.IsDraft);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ObstacleCompleteModel");
        }
    }
}
