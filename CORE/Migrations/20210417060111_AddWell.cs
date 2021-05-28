using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BeicipFranLabERP.Migrations
{
    public partial class AddWell : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Wells",
                columns: table => new
                {
                    WellId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    IconUrl = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Alias = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    BlockName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    FieldName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    BasinName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Latitude = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    XEasting = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    YNorthing = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SpudDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TD = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WellType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MDDF = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TVDDF = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wells", x => x.WellId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Wells");
        }
    }
}
