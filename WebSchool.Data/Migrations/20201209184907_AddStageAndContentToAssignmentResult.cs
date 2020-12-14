using Microsoft.EntityFrameworkCore.Migrations;

namespace WebSchool.Migrations
{
    public partial class AddStageAndContentToAssignmentResult : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "AssignmentResults",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte>(
                name: "Stage",
                table: "AssignmentResults",
                nullable: false,
                defaultValue: (byte)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "AssignmentResults");

            migrationBuilder.DropColumn(
                name: "Stage",
                table: "AssignmentResults");
        }
    }
}
