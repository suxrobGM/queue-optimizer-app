using Microsoft.EntityFrameworkCore.Migrations;

namespace MedicalClinicQueue.Migrations
{
    public partial class renamed_timestamp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Timestamp",
                table: "ServiceItems",
                newName: "LastTimestamp");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastTimestamp",
                table: "ServiceItems",
                newName: "Timestamp");
        }
    }
}
