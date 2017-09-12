using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebCore.Migrations
{
    public partial class Culture_Default_Default_value : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Default",
                table: "Culture",
                nullable: false,
                defaultValueSql: "(0)",
                oldClrType: typeof(bool));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Default",
                table: "Culture",
                nullable: false,
                oldClrType: typeof(bool),
                oldDefaultValueSql: "(0)");
        }
    }
}
