using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Medik.Migrations
{
    /// <inheritdoc />
    public partial class AddValidationToPatient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Examinations_Patients_PatientId",
                table: "Examinations");

            migrationBuilder.DropForeignKey(
                name: "FK_MedDocumentations_Patients_PatientId",
                table: "MedDocumentations");

            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_Patients_PatientId",
                table: "Prescriptions");

            migrationBuilder.DropIndex(
                name: "IX_Prescriptions_PatientId",
                table: "Prescriptions");

            migrationBuilder.DropIndex(
                name: "IX_MedDocumentations_PatientId",
                table: "MedDocumentations");

            migrationBuilder.DropIndex(
                name: "IX_Examinations_PatientId",
                table: "Examinations");

            migrationBuilder.AddColumn<long>(
                name: "PatientId1",
                table: "Prescriptions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "Patients",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<long>(
                name: "PatientId1",
                table: "MedDocumentations",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "PatientId1",
                table: "Examinations",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_PatientId1",
                table: "Prescriptions",
                column: "PatientId1");

            migrationBuilder.CreateIndex(
                name: "IX_MedDocumentations_PatientId1",
                table: "MedDocumentations",
                column: "PatientId1");

            migrationBuilder.CreateIndex(
                name: "IX_Examinations_PatientId1",
                table: "Examinations",
                column: "PatientId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Examinations_Patients_PatientId1",
                table: "Examinations",
                column: "PatientId1",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MedDocumentations_Patients_PatientId1",
                table: "MedDocumentations",
                column: "PatientId1",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Prescriptions_Patients_PatientId1",
                table: "Prescriptions",
                column: "PatientId1",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Examinations_Patients_PatientId1",
                table: "Examinations");

            migrationBuilder.DropForeignKey(
                name: "FK_MedDocumentations_Patients_PatientId1",
                table: "MedDocumentations");

            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_Patients_PatientId1",
                table: "Prescriptions");

            migrationBuilder.DropIndex(
                name: "IX_Prescriptions_PatientId1",
                table: "Prescriptions");

            migrationBuilder.DropIndex(
                name: "IX_MedDocumentations_PatientId1",
                table: "MedDocumentations");

            migrationBuilder.DropIndex(
                name: "IX_Examinations_PatientId1",
                table: "Examinations");

            migrationBuilder.DropColumn(
                name: "PatientId1",
                table: "Prescriptions");

            migrationBuilder.DropColumn(
                name: "PatientId1",
                table: "MedDocumentations");

            migrationBuilder.DropColumn(
                name: "PatientId1",
                table: "Examinations");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Patients",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_PatientId",
                table: "Prescriptions",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_MedDocumentations_PatientId",
                table: "MedDocumentations",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Examinations_PatientId",
                table: "Examinations",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Examinations_Patients_PatientId",
                table: "Examinations",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MedDocumentations_Patients_PatientId",
                table: "MedDocumentations",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Prescriptions_Patients_PatientId",
                table: "Prescriptions",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
