using Microsoft.EntityFrameworkCore.Migrations;

namespace MCBA_Web.Migrations
{
    public partial class c : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mobile",
                table: "Payees");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Payees",
                type: "nvarchar(14)",
                maxLength: 14,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Payees");

            migrationBuilder.AddColumn<string>(
                name: "Mobile",
                table: "Payees",
                type: "nvarchar(11)",
                maxLength: 11,
                nullable: true);
        }
    }
}
