﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlataformaCursosAPI.Migrations
{
    /// <inheritdoc />
    public partial class RemoverTipoUsuarioDestinoDeComunicado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TipoUsuarioDestino",
                table: "Comunicados");

            migrationBuilder.AlterColumn<string>(
                name: "UrlImagem",
                table: "Comunicados",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Comunicados",
                keyColumn: "UrlImagem",
                keyValue: null,
                column: "UrlImagem",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "UrlImagem",
                table: "Comunicados",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "TipoUsuarioDestino",
                table: "Comunicados",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
