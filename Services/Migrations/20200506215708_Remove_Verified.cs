using Microsoft.EntityFrameworkCore.Migrations;

namespace Services.Migrations
{
	public partial class Remove_Verified : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "Verified",
				table: "Users");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<bool>(
				name: "Verified",
				table: "Users",
				type: "bit",
				nullable: false,
				defaultValue: false);
		}
	}
}