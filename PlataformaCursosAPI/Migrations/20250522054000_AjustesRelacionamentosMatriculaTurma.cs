using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlataformaCursosAPI.Migrations
{
    /// <inheritdoc />
    public partial class AjustesRelacionamentosMatriculaTurma : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matriculas_Alunos_AlunoId",
                table: "Matriculas");

            migrationBuilder.DropForeignKey(
                name: "FK_Matriculas_Cursos_CursoId",
                table: "Matriculas");

            migrationBuilder.DropIndex(
                name: "IX_Matriculas_AlunoId",
                table: "Matriculas");

            migrationBuilder.DropColumn(
                name: "AlunoId",
                table: "Matriculas");

            migrationBuilder.RenameColumn(
                name: "CursoId",
                table: "Matriculas",
                newName: "PessoaId");

            migrationBuilder.RenameIndex(
                name: "IX_Matriculas_CursoId",
                table: "Matriculas",
                newName: "IX_Matriculas_PessoaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Matriculas_Pessoas_PessoaId",
                table: "Matriculas",
                column: "PessoaId",
                principalTable: "Pessoas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matriculas_Pessoas_PessoaId",
                table: "Matriculas");

            migrationBuilder.RenameColumn(
                name: "PessoaId",
                table: "Matriculas",
                newName: "CursoId");

            migrationBuilder.RenameIndex(
                name: "IX_Matriculas_PessoaId",
                table: "Matriculas",
                newName: "IX_Matriculas_CursoId");

            migrationBuilder.AddColumn<int>(
                name: "AlunoId",
                table: "Matriculas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Matriculas_AlunoId",
                table: "Matriculas",
                column: "AlunoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Matriculas_Alunos_AlunoId",
                table: "Matriculas",
                column: "AlunoId",
                principalTable: "Alunos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Matriculas_Cursos_CursoId",
                table: "Matriculas",
                column: "CursoId",
                principalTable: "Cursos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
