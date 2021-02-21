using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Services.Migrations
{
    public partial class User_FacebookJsonData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FacebookJsonData",
                table: "Users",
                type: "nvarchar(max)",
                maxLength: 2147483647,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "OAuthProviders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_OAuthProviders_UserId",
                table: "OAuthProviders",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_OAuthProviders_Users_UserId",
                table: "OAuthProviders",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OAuthProviders_Users_UserId",
                table: "OAuthProviders");

            migrationBuilder.DropIndex(
                name: "IX_OAuthProviders_UserId",
                table: "OAuthProviders");

            migrationBuilder.DropColumn(
                name: "FacebookJsonData",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "OAuthProviders");
        }
    }
}
