using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlataformaCursosAPI.Migrations
{
    /// <inheritdoc />
    public partial class Mudando : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descricao",
                table: "Cursos");

            migrationBuilder.DropColumn(
                name: "DuracaoEmHoras",
                table: "Cursos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Descricao",
                table: "Cursos",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "DuracaoEmHoras",
                table: "Cursos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
