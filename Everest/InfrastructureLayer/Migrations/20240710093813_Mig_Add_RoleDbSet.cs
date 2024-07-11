using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfrastructureLayer.Migrations
{
    public partial class Mig_Add_RoleDbSet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseUser_Courses_CourseId",
                table: "CourseUser");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseUser_Users_UserId",
                table: "CourseUser");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleUser_Role_RoleId",
                table: "RoleUser");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleUser_Users_UserId",
                table: "RoleUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoleUser",
                table: "RoleUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseUser",
                table: "CourseUser");

            migrationBuilder.RenameTable(
                name: "RoleUser",
                newName: "RoleUsers");

            migrationBuilder.RenameTable(
                name: "CourseUser",
                newName: "CourseUsers");

            migrationBuilder.RenameIndex(
                name: "IX_RoleUser_RoleId",
                table: "RoleUsers",
                newName: "IX_RoleUsers_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseUser_CourseId",
                table: "CourseUsers",
                newName: "IX_CourseUsers_CourseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoleUsers",
                table: "RoleUsers",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseUsers",
                table: "CourseUsers",
                columns: new[] { "UserId", "CourseId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CourseUsers_Courses_CourseId",
                table: "CourseUsers",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseUsers_Users_UserId",
                table: "CourseUsers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RoleUsers_Role_RoleId",
                table: "RoleUsers",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RoleUsers_Users_UserId",
                table: "RoleUsers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseUsers_Courses_CourseId",
                table: "CourseUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseUsers_Users_UserId",
                table: "CourseUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleUsers_Role_RoleId",
                table: "RoleUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleUsers_Users_UserId",
                table: "RoleUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoleUsers",
                table: "RoleUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseUsers",
                table: "CourseUsers");

            migrationBuilder.RenameTable(
                name: "RoleUsers",
                newName: "RoleUser");

            migrationBuilder.RenameTable(
                name: "CourseUsers",
                newName: "CourseUser");

            migrationBuilder.RenameIndex(
                name: "IX_RoleUsers_RoleId",
                table: "RoleUser",
                newName: "IX_RoleUser_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseUsers_CourseId",
                table: "CourseUser",
                newName: "IX_CourseUser_CourseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoleUser",
                table: "RoleUser",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseUser",
                table: "CourseUser",
                columns: new[] { "UserId", "CourseId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CourseUser_Courses_CourseId",
                table: "CourseUser",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseUser_Users_UserId",
                table: "CourseUser",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RoleUser_Role_RoleId",
                table: "RoleUser",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RoleUser_Users_UserId",
                table: "RoleUser",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
