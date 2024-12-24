using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Padiatric.Migrations
{
    /// <inheritdoc />
    public partial class jsjsj : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Assistants_AssistantId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Professors_ProfessorId",
                table: "Appointments");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Assistants_AssistantId",
                table: "Appointments",
                column: "AssistantId",
                principalTable: "Assistants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Professors_ProfessorId",
                table: "Appointments",
                column: "ProfessorId",
                principalTable: "Professors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Assistants_AssistantId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Professors_ProfessorId",
                table: "Appointments");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Assistants_AssistantId",
                table: "Appointments",
                column: "AssistantId",
                principalTable: "Assistants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Professors_ProfessorId",
                table: "Appointments",
                column: "ProfessorId",
                principalTable: "Professors",
                principalColumn: "Id");
        }
    }
}
