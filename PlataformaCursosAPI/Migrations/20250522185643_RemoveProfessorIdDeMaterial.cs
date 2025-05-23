using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlataformaCursosAPI.Migrations
{
    public partial class RemoveProfessorIdDeMaterial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Materiais_Professores_ProfessorId",
                table: "Materiais");

            migrationBuilder.DropIndex(
                name: "IX_Materiais_ProfessorId",
                table: "Materiais");

            migrationBuilder.DropColumn(
                name: "ProfessorId",
                table: "Materiais");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProfessorId",
                table: "Materiais",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Materiais_ProfessorId",
                table: "Materiais",
                column: "ProfessorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Materiais_Professores_ProfessorId",
                table: "Materiais",
                column: "ProfessorId",
                principalTable: "Professores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

