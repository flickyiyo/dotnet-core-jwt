using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace courses_platform.Migrations
{
    public partial class ChangedPasswordToBytes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "Password",
                table: "Users",
                type: "longblob",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Users",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "longblob")
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
