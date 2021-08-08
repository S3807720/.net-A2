using Microsoft.EntityFrameworkCore.Migrations;

namespace MCBA_Web.Migrations
{
    public partial class loginupdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanLogin",
                table: "Logins");

            migrationBuilder.AddColumn<bool>(
                name: "CanLogin",
                table: "Customers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanLogin",
                table: "Customers");

            migrationBuilder.AddColumn<bool>(
                name: "CanLogin",
                table: "Logins",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
