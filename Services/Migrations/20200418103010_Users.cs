using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Services.Migrations
{
    public partial class Users : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ErrorLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateAdded = table.Column<DateTime>(nullable: false),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    Type = table.Column<string>(maxLength: 100, nullable: false),
                    Message = table.Column<string>(maxLength: 5000, nullable: false),
                    Exception = table.Column<string>(maxLength: 2147483647, nullable: false),
                    LocationInCode = table.Column<string>(maxLength: 500, nullable: true),
                    RequestDetails = table.Column<string>(maxLength: 2147483647, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OAuthProviders",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateAdded = table.Column<DateTime>(nullable: false),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    ProviderName = table.Column<string>(maxLength: 100, nullable: false),
                    Email = table.Column<string>(maxLength: 350, nullable: false),
                    DataJson = table.Column<string>(maxLength: 2147483647, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OAuthProviders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateAdded = table.Column<DateTime>(nullable: false),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(maxLength: 350, nullable: false),
                    Username = table.Column<string>(maxLength: 200, nullable: false),
                    Password = table.Column<string>(maxLength: 5000, nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    Gender = table.Column<string>(maxLength: 50, nullable: true),
                    Surname = table.Column<string>(maxLength: 200, nullable: false),
                    AccountAuth = table.Column<string>(maxLength: 100, nullable: false),
                    Country = table.Column<string>(maxLength: 200, nullable: true),
                    About = table.Column<string>(maxLength: 2147483647, nullable: true),
                    Verified = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ErrorLogs");

            migrationBuilder.DropTable(
                name: "OAuthProviders");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
