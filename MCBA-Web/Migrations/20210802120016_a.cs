using Microsoft.EntityFrameworkCore.Migrations;

namespace MCBA_Web.Migrations
{
    public partial class a : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BillPay",
                table: "BillPay");

            migrationBuilder.RenameTable(
                name: "BillPay",
                newName: "BillPays");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BillPays",
                table: "BillPays",
                column: "BillPayID");

            migrationBuilder.CreateTable(
                name: "Payees",
                columns: table => new
                {
                    PayeeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Suburb = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    State = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    PostCode = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    Mobile = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payees", x => x.PayeeID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BillPays",
                table: "BillPays");

            migrationBuilder.RenameTable(
                name: "BillPays",
                newName: "BillPay");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BillPay",
                table: "BillPay",
                column: "BillPayID");
        }
    }
}
