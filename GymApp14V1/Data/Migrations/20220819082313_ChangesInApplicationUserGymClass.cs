using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymApp14V1.Data.Migrations
{
    public partial class ChangesInApplicationUserGymClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserGymClass_AspNetUsers_ApplicationUserId",
                table: "ApplicationUserGymClass");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserGymClass_GymPasses_GymPassId",
                table: "ApplicationUserGymClass");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicationUserGymClass",
                table: "ApplicationUserGymClass");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationUserGymClass_ApplicationUserId",
                table: "ApplicationUserGymClass");

            migrationBuilder.DropColumn(
                name: "MemberId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "GymPassId",
                table: "ApplicationUserGymClass");

            migrationBuilder.RenameColumn(
                name: "GymPassId",
                table: "GymPasses",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "MemberId",
                table: "ApplicationUserGymClass",
                newName: "GymClassId");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ApplicationUserId",
                table: "ApplicationUserGymClass",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId1",
                table: "ApplicationUserGymClass",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicationUserGymClass",
                table: "ApplicationUserGymClass",
                columns: new[] { "GymClassId", "ApplicationUserId" });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserGymClass_ApplicationUserId1",
                table: "ApplicationUserGymClass",
                column: "ApplicationUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserGymClass_AspNetUsers_ApplicationUserId1",
                table: "ApplicationUserGymClass",
                column: "ApplicationUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserGymClass_GymPasses_GymClassId",
                table: "ApplicationUserGymClass",
                column: "GymClassId",
                principalTable: "GymPasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserGymClass_AspNetUsers_ApplicationUserId1",
                table: "ApplicationUserGymClass");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserGymClass_GymPasses_GymClassId",
                table: "ApplicationUserGymClass");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicationUserGymClass",
                table: "ApplicationUserGymClass");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationUserGymClass_ApplicationUserId1",
                table: "ApplicationUserGymClass");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId1",
                table: "ApplicationUserGymClass");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "GymPasses",
                newName: "GymPassId");

            migrationBuilder.RenameColumn(
                name: "GymClassId",
                table: "ApplicationUserGymClass",
                newName: "MemberId");

            migrationBuilder.AddColumn<int>(
                name: "MemberId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "ApplicationUserGymClass",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "GymPassId",
                table: "ApplicationUserGymClass",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicationUserGymClass",
                table: "ApplicationUserGymClass",
                columns: new[] { "GymPassId", "MemberId" });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserGymClass_ApplicationUserId",
                table: "ApplicationUserGymClass",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserGymClass_AspNetUsers_ApplicationUserId",
                table: "ApplicationUserGymClass",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserGymClass_GymPasses_GymPassId",
                table: "ApplicationUserGymClass",
                column: "GymPassId",
                principalTable: "GymPasses",
                principalColumn: "GymPassId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
