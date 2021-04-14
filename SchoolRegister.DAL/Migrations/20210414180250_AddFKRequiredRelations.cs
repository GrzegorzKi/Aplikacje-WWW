using Microsoft.EntityFrameworkCore.Migrations;

namespace SchoolRegister.DAL.Migrations
{
    public partial class AddFKRequiredRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_ParentId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_AspNetUsers_TeacherId",
                table: "Subjects");

            migrationBuilder.AlterColumn<int>(
                name: "TeacherId",
                table: "Subjects",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_ParentId",
                table: "AspNetUsers",
                column: "ParentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_AspNetUsers_TeacherId",
                table: "Subjects",
                column: "TeacherId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_ParentId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_AspNetUsers_TeacherId",
                table: "Subjects");

            migrationBuilder.AlterColumn<int>(
                name: "TeacherId",
                table: "Subjects",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_ParentId",
                table: "AspNetUsers",
                column: "ParentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_AspNetUsers_TeacherId",
                table: "Subjects",
                column: "TeacherId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
