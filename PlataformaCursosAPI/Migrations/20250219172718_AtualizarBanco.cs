using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlataformaCursosAPI.Migrations
{
    /// <inheritdoc />
    public partial class AtualizarBanco : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alunos_Cursos_CursoId",
                table: "Alunos");

            migrationBuilder.DropForeignKey(
                name: "FK_Professores_Cursos_CursoId",
                table: "Professores");

            migrationBuilder.DropForeignKey(
                name: "FK_Professores_Cursos_CursoId1",
                table: "Professores");

            migrationBuilder.DropIndex(
                name: "IX_Professores_CursoId",
                table: "Professores");

            migrationBuilder.DropIndex(
                name: "IX_Professores_CursoId1",
                table: "Professores");

            migrationBuilder.DropIndex(
                name: "IX_Alunos_CursoId",
                table: "Alunos");

            migrationBuilder.DropColumn(
                name: "CursoId",
                table: "Professores");

            migrationBuilder.DropColumn(
                name: "CursoId1",
                table: "Professores");

            migrationBuilder.DropColumn(
                name: "CursoId",
                table: "Alunos");

            migrationBuilder.DropColumn(
                name: "DataNascimento",
                table: "Alunos");

            migrationBuilder.AddColumn<string>(
                name: "Matricula",
                table: "Alunos",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Matricula",
                table: "Alunos");

            migrationBuilder.AddColumn<int>(
                name: "CursoId",
                table: "Professores",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CursoId1",
                table: "Professores",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CursoId",
                table: "Alunos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataNascimento",
                table: "Alunos",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Professores_CursoId",
                table: "Professores",
                column: "CursoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Professores_CursoId1",
                table: "Professores",
                column: "CursoId1");

            migrationBuilder.CreateIndex(
                name: "IX_Alunos_CursoId",
                table: "Alunos",
                column: "CursoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Alunos_Cursos_CursoId",
                table: "Alunos",
                column: "CursoId",
                principalTable: "Cursos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Professores_Cursos_CursoId",
                table: "Professores",
                column: "CursoId",
                principalTable: "Cursos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Professores_Cursos_CursoId1",
                table: "Professores",
                column: "CursoId1",
                principalTable: "Cursos",
                principalColumn: "Id");
        }
    }
}
