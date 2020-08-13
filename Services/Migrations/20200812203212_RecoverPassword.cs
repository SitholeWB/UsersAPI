using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Services.Migrations
{
	public partial class RecoverPassword : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "RecoverPasswords",
				columns: table => new
				{
					Id = table.Column<Guid>(nullable: false),
					DateAdded = table.Column<DateTime>(nullable: false),
					LastModifiedDate = table.Column<DateTime>(nullable: false),
					Email = table.Column<string>(maxLength: 350, nullable: false),
					Hash = table.Column<string>(maxLength: 5000, nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_RecoverPasswords", x => x.Id);
				});
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "RecoverPasswords");
		}
	}
}
