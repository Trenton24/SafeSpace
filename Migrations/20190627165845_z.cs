using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SafeSpace.Migrations
{
    public partial class z : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Friends",
                columns: table => new
                {
                    FriendId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user1Id = table.Column<int>(nullable: false),
                    user2Id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friends", x => x.FriendId);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "UserhasFriends",
                columns: table => new
                {
                    RelationshipId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RequestedId = table.Column<int>(nullable: false),
                    Accepted = table.Column<bool>(nullable: false),
                    SentById = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserhasFriends", x => x.RelationshipId);
                    table.ForeignKey(
                        name: "FK_UserhasFriends_users_RequestedId",
                        column: x => x.RequestedId,
                        principalTable: "users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserhasFriends_users_SentById",
                        column: x => x.SentById,
                        principalTable: "users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserhasFriends_RequestedId",
                table: "UserhasFriends",
                column: "RequestedId");

            migrationBuilder.CreateIndex(
                name: "IX_UserhasFriends_SentById",
                table: "UserhasFriends",
                column: "SentById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Friends");

            migrationBuilder.DropTable(
                name: "UserhasFriends");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
