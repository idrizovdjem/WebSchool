using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebSchool.Data.Migrations
{
    public partial class ClearUserClassTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "UserClasses");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "UserClasses");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "UserClasses");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "UserClasses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "UserClasses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "UserClasses",
                type: "datetime2",
                nullable: true);
        }
    }
}
