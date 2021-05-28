using Microsoft.EntityFrameworkCore.Migrations;

namespace BeicipFranLabERP.Migrations
{
    public partial class secondadminSetting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MainFolderOnNetwork",
                table: "AdministratorSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MainFolderOnNetwork",
                table: "AdministratorSettings");
        }
    }
}
