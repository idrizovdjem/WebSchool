using Microsoft.EntityFrameworkCore.Migrations;

namespace WebSchool.Data.Migrations
{
    public partial class AddGroupAssignmentIdToAssignmentResultTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AssignmentId",
                table: "AssignmentResults",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "GroupAssignmentId",
                table: "AssignmentResults",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentResults_GroupAssignmentId",
                table: "AssignmentResults",
                column: "GroupAssignmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssignmentResults_GroupAssignments_GroupAssignmentId",
                table: "AssignmentResults",
                column: "GroupAssignmentId",
                principalTable: "GroupAssignments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssignmentResults_GroupAssignments_GroupAssignmentId",
                table: "AssignmentResults");

            migrationBuilder.DropIndex(
                name: "IX_AssignmentResults_GroupAssignmentId",
                table: "AssignmentResults");

            migrationBuilder.DropColumn(
                name: "GroupAssignmentId",
                table: "AssignmentResults");

            migrationBuilder.AlterColumn<string>(
                name: "AssignmentId",
                table: "AssignmentResults",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
