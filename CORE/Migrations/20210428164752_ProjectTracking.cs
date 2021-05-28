using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BeicipFranLabERP.Migrations
{
    public partial class ProjectTracking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectTrackers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MilestoneName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MilestoneDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DaystoML = table.Column<int>(type: "int", nullable: false),
                    ExistingPercentage = table.Column<int>(type: "int", nullable: false),
                    ProposedPersentage = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTrackers", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectTrackers");
        }
    }
}
