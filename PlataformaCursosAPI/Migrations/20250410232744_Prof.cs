using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlataformaCursosAPI.Migrations
{
    /// <inheritdoc />
    public partial class Prof : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CursoId",
                table: "Professores",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Professores_CursoId",
                table: "Professores",
                column: "CursoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Professores_Cursos_CursoId",
                table: "Professores",
                column: "CursoId",
                principalTable: "Cursos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Professores_Cursos_CursoId",
                table: "Professores");

            migrationBuilder.DropIndex(
                name: "IX_Professores_CursoId",
                table: "Professores");

            migrationBuilder.DropColumn(
                name: "CursoId",
                table: "Professores");
        }
    }
}
