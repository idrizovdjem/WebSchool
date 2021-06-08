using Microsoft.EntityFrameworkCore.Migrations;

namespace WebSchool.Data.Migrations
{
    public partial class RenameUserAssignmentsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAssignments_Assignments_AssignmentId",
                table: "UserAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAssignments_Groups_GroupId",
                table: "UserAssignments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAssignments",
                table: "UserAssignments");

            migrationBuilder.RenameTable(
                name: "UserAssignments",
                newName: "GroupAssignments");

            migrationBuilder.RenameIndex(
                name: "IX_UserAssignments_AssignmentId",
                table: "GroupAssignments",
                newName: "IX_GroupAssignments_AssignmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupAssignments",
                table: "GroupAssignments",
                columns: new[] { "GroupId", "AssignmentId" });

            migrationBuilder.AddForeignKey(
                name: "FK_GroupAssignments_Assignments_AssignmentId",
                table: "GroupAssignments",
                column: "AssignmentId",
                principalTable: "Assignments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupAssignments_Groups_GroupId",
                table: "GroupAssignments",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupAssignments_Assignments_AssignmentId",
                table: "GroupAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupAssignments_Groups_GroupId",
                table: "GroupAssignments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupAssignments",
                table: "GroupAssignments");

            migrationBuilder.RenameTable(
                name: "GroupAssignments",
                newName: "UserAssignments");

            migrationBuilder.RenameIndex(
                name: "IX_GroupAssignments_AssignmentId",
                table: "UserAssignments",
                newName: "IX_UserAssignments_AssignmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAssignments",
                table: "UserAssignments",
                columns: new[] { "GroupId", "AssignmentId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserAssignments_Assignments_AssignmentId",
                table: "UserAssignments",
                column: "AssignmentId",
                principalTable: "Assignments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAssignments_Groups_GroupId",
                table: "UserAssignments",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
