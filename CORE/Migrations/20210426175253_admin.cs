using Microsoft.EntityFrameworkCore.Migrations;

namespace BeicipFranLabERP.Migrations
{
    public partial class admin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdministratorSettings",
                columns: table => new
                {
                    AdministratorSettingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DuplicateDocumentDetection = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InsertSpecialCharacters = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DocumentPathMaxLength = table.Column<int>(type: "int", nullable: false),
                    MaxDocumentSize = table.Column<int>(type: "int", nullable: false),
                    MinCharsforExtractedDocContent = table.Column<int>(type: "int", nullable: false),
                    DefaultCategory = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DefaultSubcategory = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DefaultEvent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DefaultLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DefaultWell = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdministratorSettings", x => x.AdministratorSettingId);
                });

            migrationBuilder.CreateTable(
                name: "CORE_Email_Templates",
                columns: table => new
                {
                    CORE_Email_TemplateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateAutoMatchRegexes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailTemplateName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailSubject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailBody = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailSignBottomPartoftheEmailBody = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CORE_Email_Templates", x => x.CORE_Email_TemplateId);
                });

            migrationBuilder.CreateTable(
                name: "PortalSettings",
                columns: table => new
                {
                    PortalSettingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyLogo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PortalLogo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Favicon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LoginPageBackground = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SiteTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CorePortalShortName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PortalSettings", x => x.PortalSettingId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdministratorSettings");

            migrationBuilder.DropTable(
                name: "CORE_Email_Templates");

            migrationBuilder.DropTable(
                name: "PortalSettings");
        }
    }
}
