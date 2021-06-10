using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebSchool.Data.Migrations
{
    public partial class ChangeGroupAssignmentsNameToGivenAssignment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssignmentResults_GroupAssignments_groupAssignmentId",
                table: "AssignmentResults");

            migrationBuilder.DropTable(
                name: "GroupAssignments");

            migrationBuilder.CreateTable(
                name: "GivenAssignments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AssignmentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GroupId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GivenAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GivenAssignments_Assignments_AssignmentId",
                        column: x => x.AssignmentId,
                        principalTable: "Assignments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GivenAssignments_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GivenAssignments_AssignmentId",
                table: "GivenAssignments",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_GivenAssignments_GroupId",
                table: "GivenAssignments",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssignmentResults_GivenAssignments_groupAssignmentId",
                table: "AssignmentResults",
                column: "groupAssignmentId",
                principalTable: "GivenAssignments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssignmentResults_GivenAssignments_groupAssignmentId",
                table: "AssignmentResults");

            migrationBuilder.DropTable(
                name: "GivenAssignments");

            migrationBuilder.CreateTable(
                name: "GroupAssignments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AssignmentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GroupId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupAssignments_Assignments_AssignmentId",
                        column: x => x.AssignmentId,
                        principalTable: "Assignments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GroupAssignments_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupAssignments_AssignmentId",
                table: "GroupAssignments",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupAssignments_GroupId",
                table: "GroupAssignments",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssignmentResults_GroupAssignments_groupAssignmentId",
                table: "AssignmentResults",
                column: "groupAssignmentId",
                principalTable: "GroupAssignments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
