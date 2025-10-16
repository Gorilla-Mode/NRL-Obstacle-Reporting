using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NRLObstacleReporting.Database.Migrations
{
    /// <inheritdoc />
    public partial class Newtabels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ObstacleCompleteModel");

            migrationBuilder.CreateTable(
                name: "ObstacleTypeDatamodels",
                columns: table => new
                {
                    ObstacleTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ObstacleName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObstacleTypeDatamodels", x => x.ObstacleTypeId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PilotDatamodels",
                columns: table => new
                {
                    PilotId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Organization = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PilotDatamodels", x => x.PilotId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RegistrarDatamodels",
                columns: table => new
                {
                    RegistrarId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Role = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Organization = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrarDatamodels", x => x.RegistrarId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ObstacleDatamodels",
                columns: table => new
                {
                    ObstacleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDraft = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ObstacleTypeId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_ObstacleDatamodels", x => x.ObstacleId);
                    table.ForeignKey(
                        name: "FK_ObstacleDatamodels_ObstacleTypeDatamodels_ObstacleTypeId",
                        column: x => x.ObstacleTypeId,
                        principalTable: "ObstacleTypeDatamodels",
                        principalColumn: "ObstacleTypeId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ReportDatamodels",
                columns: table => new
                {
                    ReportId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Dato = table.Column<DateTime>(type: "date", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PilotId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportDatamodels", x => x.ReportId);
                    table.ForeignKey(
                        name: "FK_ReportDatamodels_PilotDatamodels_PilotId",
                        column: x => x.PilotId,
                        principalTable: "PilotDatamodels",
                        principalColumn: "PilotId",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ObstacleDatamodels_ObstacleTypeId",
                table: "ObstacleDatamodels",
                column: "ObstacleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportDatamodels_PilotId",
                table: "ReportDatamodels",
                column: "PilotId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ObstacleDatamodels");

            migrationBuilder.DropTable(
                name: "RegistrarDatamodels");

            migrationBuilder.DropTable(
                name: "ReportDatamodels");

            migrationBuilder.DropTable(
                name: "ObstacleTypeDatamodels");

            migrationBuilder.DropTable(
                name: "PilotDatamodels");

            migrationBuilder.CreateTable(
                name: "ObstacleCompleteModel",
                columns: table => new
                {
                    IsDraft = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    GeometryGeoJson = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ObstacleDescription = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ObstacleHeightMeter = table.Column<int>(type: "int", nullable: false),
                    ObstacleId = table.Column<int>(type: "int", nullable: false),
                    ObstacleIlluminated = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    ObstacleName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObstacleCompleteModel", x => x.IsDraft);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
