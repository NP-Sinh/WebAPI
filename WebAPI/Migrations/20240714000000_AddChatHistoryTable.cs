using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    public partial class AddChatHistoryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChatHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    UserMessage = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    BotResponse = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ChatHistory__3214EC07", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatHistory_NguoiDung",
                        column: x => x.UserId,
                        principalTable: "NguoiDung",
                        principalColumn: "MaNguoiDung");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatHistory_UserId",
                table: "ChatHistory",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatHistory");
        }
    }
} 