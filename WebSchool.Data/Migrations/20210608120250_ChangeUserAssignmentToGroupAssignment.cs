using Microsoft.EntityFrameworkCore.Migrations;

namespace WebSchool.Data.Migrations
{
    public partial class ChangeUserAssignmentToGroupAssignment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAssignments_AspNetUsers_StudentId",
                table: "UserAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAssignments_Groups_GroupId",
                table: "UserAssignments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAssignments",
                table: "UserAssignments");

            migrationBuilder.DropIndex(
                name: "IX_UserAssignments_GroupId",
                table: "UserAssignments");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "UserAssignments");

            migrationBuilder.DropColumn(
                name: "IsSolved",
                table: "UserAssignments");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAssignments",
                table: "UserAssignments",
                columns: new[] { "GroupId", "AssignmentId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserAssignments_Groups_GroupId",
                table: "UserAssignments",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAssignments_Groups_GroupId",
                table: "UserAssignments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAssignments",
                table: "UserAssignments");

            migrationBuilder.AddColumn<string>(
                name: "StudentId",
                table: "UserAssignments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsSolved",
                table: "UserAssignments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAssignments",
                table: "UserAssignments",
                columns: new[] { "StudentId", "AssignmentId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserAssignments_GroupId",
                table: "UserAssignments",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAssignments_AspNetUsers_StudentId",
                table: "UserAssignments",
                column: "StudentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAssignments_Groups_GroupId",
                table: "UserAssignments",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
