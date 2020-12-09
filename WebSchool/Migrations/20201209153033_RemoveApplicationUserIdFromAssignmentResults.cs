using Microsoft.EntityFrameworkCore.Migrations;

namespace WebSchool.Migrations
{
    public partial class RemoveApplicationUserIdFromAssignmentResults : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssignmentResults_AspNetUsers_ApplicationUserId",
                table: "AssignmentResults");

            migrationBuilder.DropForeignKey(
                name: "FK_AssignmentResults_Assignments_AssignmentId",
                table: "AssignmentResults");

            migrationBuilder.DropIndex(
                name: "IX_AssignmentResults_ApplicationUserId",
                table: "AssignmentResults");

            migrationBuilder.DropIndex(
                name: "IX_AssignmentResults_AssignmentId",
                table: "AssignmentResults");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "AssignmentResults");

            migrationBuilder.AlterColumn<string>(
                name: "AssignmentId",
                table: "AssignmentResults",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AssignmentId",
                table: "AssignmentResults",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "AssignmentResults",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentResults_ApplicationUserId",
                table: "AssignmentResults",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentResults_AssignmentId",
                table: "AssignmentResults",
                column: "AssignmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssignmentResults_AspNetUsers_ApplicationUserId",
                table: "AssignmentResults",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AssignmentResults_Assignments_AssignmentId",
                table: "AssignmentResults",
                column: "AssignmentId",
                principalTable: "Assignments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
