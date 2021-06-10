using Microsoft.EntityFrameworkCore.Migrations;

namespace WebSchool.Data.Migrations
{
    public partial class AddgroupAssignmentIdToAssignmentResultTable : Migration
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
                name: "groupAssignmentId",
                table: "AssignmentResults",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentResults_groupAssignmentId",
                table: "AssignmentResults",
                column: "groupAssignmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssignmentResults_GroupAssignments_groupAssignmentId",
                table: "AssignmentResults",
                column: "groupAssignmentId",
                principalTable: "GroupAssignments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssignmentResults_GroupAssignments_groupAssignmentId",
                table: "AssignmentResults");

            migrationBuilder.DropIndex(
                name: "IX_AssignmentResults_groupAssignmentId",
                table: "AssignmentResults");

            migrationBuilder.DropColumn(
                name: "groupAssignmentId",
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
