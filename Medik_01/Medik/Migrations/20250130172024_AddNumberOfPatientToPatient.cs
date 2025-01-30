using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Medik.Migrations
{
    /// <inheritdoc />
    public partial class AddNumberOfPatientToPatient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NumberOfPatient",
                table: "Patients",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfPatient",
                table: "Patients");
        }
    }
}
