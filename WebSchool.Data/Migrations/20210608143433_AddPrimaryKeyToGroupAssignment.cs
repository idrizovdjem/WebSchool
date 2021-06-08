using Microsoft.EntityFrameworkCore.Migrations;

namespace WebSchool.Data.Migrations
{
    public partial class AddPrimaryKeyToGroupAssignment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupAssignments",
                table: "GroupAssignments");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "GroupAssignments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupAssignments",
                table: "GroupAssignments",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_GroupAssignments_GroupId",
                table: "GroupAssignments",
                column: "GroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupAssignments",
                table: "GroupAssignments");

            migrationBuilder.DropIndex(
                name: "IX_GroupAssignments_GroupId",
                table: "GroupAssignments");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "GroupAssignments");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupAssignments",
                table: "GroupAssignments",
                columns: new[] { "GroupId", "AssignmentId" });
        }
    }
}
