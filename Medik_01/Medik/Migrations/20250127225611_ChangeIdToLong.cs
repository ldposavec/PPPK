using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Medik.Migrations
{
    /// <inheritdoc />
    public partial class ChangeIdToLong : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Examinations_Patients_PatientId1",
                table: "Examinations");

            migrationBuilder.AlterColumn<long>(
                name: "PatientId1",
                table: "Examinations",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_Examinations_Patients_PatientId1",
                table: "Examinations",
                column: "PatientId1",
                principalTable: "Patients",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Examinations_Patients_PatientId1",
                table: "Examinations");

            migrationBuilder.AlterColumn<long>(
                name: "PatientId1",
                table: "Examinations",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Examinations_Patients_PatientId1",
                table: "Examinations",
                column: "PatientId1",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
