using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebSchool.Data.Migrations
{
    public partial class ChangeAssignmentsTableStructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignmentContent",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "AssignmentResults");

            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "AssignmentResults");

            migrationBuilder.DropColumn(
                name: "Stage",
                table: "AssignmentResults");

            migrationBuilder.RenameColumn(
                name: "Signature",
                table: "Assignments",
                newName: "Content");

            migrationBuilder.RenameColumn(
                name: "AssignmentTitle",
                table: "Assignments",
                newName: "Title");

            migrationBuilder.AddColumn<string>(
                name: "GroupId",
                table: "Assignments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "StudentId",
                table: "AssignmentResults",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "AssignmentId",
                table: "AssignmentResults",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "UserAssignments",
                columns: table => new
                {
                    StudentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AssignmentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsSolved = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAssignments", x => new { x.StudentId, x.AssignmentId });
                    table.ForeignKey(
                        name: "FK_UserAssignments_AspNetUsers_StudentId",
                        column: x => x.StudentId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserAssignments_Assignments_AssignmentId",
                        column: x => x.AssignmentId,
                        principalTable: "Assignments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_GroupId",
                table: "Assignments",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentResults_AssignmentId",
                table: "AssignmentResults",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentResults_StudentId",
                table: "AssignmentResults",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAssignments_AssignmentId",
                table: "UserAssignments",
                column: "AssignmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssignmentResults_AspNetUsers_StudentId",
                table: "AssignmentResults",
                column: "StudentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AssignmentResults_Assignments_AssignmentId",
                table: "AssignmentResults",
                column: "AssignmentId",
                principalTable: "Assignments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Groups_GroupId",
                table: "Assignments",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssignmentResults_AspNetUsers_StudentId",
                table: "AssignmentResults");

            migrationBuilder.DropForeignKey(
                name: "FK_AssignmentResults_Assignments_AssignmentId",
                table: "AssignmentResults");

            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_Groups_GroupId",
                table: "Assignments");

            migrationBuilder.DropTable(
                name: "UserAssignments");

            migrationBuilder.DropIndex(
                name: "IX_Assignments_GroupId",
                table: "Assignments");

            migrationBuilder.DropIndex(
                name: "IX_AssignmentResults_AssignmentId",
                table: "AssignmentResults");

            migrationBuilder.DropIndex(
                name: "IX_AssignmentResults_StudentId",
                table: "AssignmentResults");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Assignments");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Assignments",
                newName: "AssignmentTitle");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Assignments",
                newName: "Signature");

            migrationBuilder.AddColumn<string>(
                name: "AssignmentContent",
                table: "Assignments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "Assignments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "StudentId",
                table: "AssignmentResults",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "AssignmentId",
                table: "AssignmentResults",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "AssignmentResults",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "AssignmentResults",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<byte>(
                name: "Stage",
                table: "AssignmentResults",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }
    }
}
