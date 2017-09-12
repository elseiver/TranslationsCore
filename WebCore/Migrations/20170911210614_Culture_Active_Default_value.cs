using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebCore.Migrations
{
    public partial class Culture_Active_Default_value : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Active",
                table: "Culture",
                nullable: false,
                defaultValueSql: "(1)",
                oldClrType: typeof(bool));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Active",
                table: "Culture",
                nullable: false,
                oldClrType: typeof(bool),
                oldDefaultValueSql: "(1)");
        }
    }
}
