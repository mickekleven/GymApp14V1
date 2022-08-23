using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymApp14V1.Data.Migrations
{
    public partial class AddMemberGymClasses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserGymClass");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "GymPasses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ApplicationUsersGymClasses",
                columns: table => new
                {
                    GymClassId = table.Column<int>(type: "int", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUsersGymClasses", x => new { x.GymClassId, x.ApplicationUserId });
                    table.ForeignKey(
                        name: "FK_ApplicationUsersGymClasses_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUsersGymClasses_GymPasses_GymClassId",
                        column: x => x.GymClassId,
                        principalTable: "GymPasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUsersGymClasses_ApplicationUserId",
                table: "ApplicationUsersGymClasses",
                column: "ApplicationUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUsersGymClasses");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "GymPasses",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "ApplicationUserGymClass",
                columns: table => new
                {
                    GymClassId = table.Column<int>(type: "int", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserGymClass", x => new { x.GymClassId, x.ApplicationUserId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserGymClass_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserGymClass_GymPasses_GymClassId",
                        column: x => x.GymClassId,
                        principalTable: "GymPasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserGymClass_ApplicationUserId",
                table: "ApplicationUserGymClass",
                column: "ApplicationUserId");
        }
    }
}
