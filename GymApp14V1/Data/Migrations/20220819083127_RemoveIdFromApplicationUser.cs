using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymApp14V1.Data.Migrations
{
    public partial class RemoveIdFromApplicationUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserGymClass_AspNetUsers_ApplicationUserId1",
                table: "ApplicationUserGymClass");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId1",
                table: "ApplicationUserGymClass",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserGymClass_AspNetUsers_ApplicationUserId1",
                table: "ApplicationUserGymClass",
                column: "ApplicationUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserGymClass_AspNetUsers_ApplicationUserId1",
                table: "ApplicationUserGymClass");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId1",
                table: "ApplicationUserGymClass",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserGymClass_AspNetUsers_ApplicationUserId1",
                table: "ApplicationUserGymClass",
                column: "ApplicationUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
