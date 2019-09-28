using Microsoft.EntityFrameworkCore.Migrations;

namespace SafeSpace.Migrations
{
    public partial class zz : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Accepted",
                table: "UserhasFriends",
                nullable: false,
                oldClrType: typeof(bool));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Accepted",
                table: "UserhasFriends",
                nullable: false,
                oldClrType: typeof(int));
        }
    }
}
