using Microsoft.EntityFrameworkCore.Migrations;

namespace WebSchool.Data.Migrations
{
    public partial class RemoveGroupFromAssignment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_AspNetUsers_TeacherId",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_Groups_GroupId",
                table: "Assignments");

            migrationBuilder.DropIndex(
                name: "IX_Assignments_GroupId",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Assignments");

            migrationBuilder.RenameColumn(
                name: "TeacherId",
                table: "Assignments",
                newName: "CreatorId");

            migrationBuilder.RenameIndex(
                name: "IX_Assignments_TeacherId",
                table: "Assignments",
                newName: "IX_Assignments_CreatorId");

            migrationBuilder.AddColumn<string>(
                name: "GroupId",
                table: "UserAssignments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_UserAssignments_GroupId",
                table: "UserAssignments",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_AspNetUsers_CreatorId",
                table: "Assignments",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAssignments_Groups_GroupId",
                table: "UserAssignments",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_AspNetUsers_CreatorId",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAssignments_Groups_GroupId",
                table: "UserAssignments");

            migrationBuilder.DropIndex(
                name: "IX_UserAssignments_GroupId",
                table: "UserAssignments");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "UserAssignments");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "Assignments",
                newName: "TeacherId");

            migrationBuilder.RenameIndex(
                name: "IX_Assignments_CreatorId",
                table: "Assignments",
                newName: "IX_Assignments_TeacherId");

            migrationBuilder.AddColumn<string>(
                name: "GroupId",
                table: "Assignments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_GroupId",
                table: "Assignments",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_AspNetUsers_TeacherId",
                table: "Assignments",
                column: "TeacherId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Groups_GroupId",
                table: "Assignments",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
