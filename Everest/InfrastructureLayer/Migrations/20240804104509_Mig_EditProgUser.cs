using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfrastructureLayer.Migrations
{
    public partial class Mig_EditProgUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Progs_ProgId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ProgId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ProgId",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "ProgUsers",
                columns: table => new
                {
                    ProgId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgUsers", x => new { x.UserId, x.ProgId });
                    table.ForeignKey(
                        name: "FK_ProgUsers_Progs_ProgId",
                        column: x => x.ProgId,
                        principalTable: "Progs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProgUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProgUsers_ProgId",
                table: "ProgUsers",
                column: "ProgId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProgUsers");

            migrationBuilder.AddColumn<int>(
                name: "ProgId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_ProgId",
                table: "Users",
                column: "ProgId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Progs_ProgId",
                table: "Users",
                column: "ProgId",
                principalTable: "Progs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
