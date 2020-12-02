using Microsoft.EntityFrameworkCore.Migrations;

namespace WebSchool.Data.Migrations
{
    public partial class AddSchoolIdToSubjectTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SchoolId",
                table: "Subjects",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_SchoolId",
                table: "Subjects",
                column: "SchoolId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_Schools_SchoolId",
                table: "Subjects",
                column: "SchoolId",
                principalTable: "Schools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_Schools_SchoolId",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_Subjects_SchoolId",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "Subjects");
        }
    }
}
