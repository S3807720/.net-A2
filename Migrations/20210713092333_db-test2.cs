using Microsoft.EntityFrameworkCore.Migrations;

namespace MCBA_Web.Migrations
{
    public partial class dbtest2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Suburb",
                table: "Customers",
                newName: "City");

            migrationBuilder.AddCheckConstraint(
                name: "CH_CustomerID",
                table: "Customers",
                sql: "len(CustomerID) = 4");

            migrationBuilder.AddCheckConstraint(
                name: "CH_TFN",
                table: "Customers",
                sql: "len(TFN) = 11");

            migrationBuilder.AddCheckConstraint(
                name: "CH_Postcode",
                table: "Customers",
                sql: "len(Postcode) = 4");

            migrationBuilder.AddCheckConstraint(
                name: "CH_Mobile",
                table: "Customers",
                sql: "len(Mobile) = 12");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CH_CustomerID",
                table: "Customers");

            migrationBuilder.DropCheckConstraint(
                name: "CH_TFN",
                table: "Customers");

            migrationBuilder.DropCheckConstraint(
                name: "CH_Postcode",
                table: "Customers");

            migrationBuilder.DropCheckConstraint(
                name: "CH_Mobile",
                table: "Customers");

            migrationBuilder.RenameColumn(
                name: "City",
                table: "Customers",
                newName: "Suburb");
        }
    }
}
