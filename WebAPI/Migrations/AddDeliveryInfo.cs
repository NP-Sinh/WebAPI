using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAPI.Migrations
{
    public partial class AddDeliveryInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DiaChiGiaoHang",
                table: "DonHang",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SoDienThoai",
                table: "DonHang",
                type: "varchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GhiChu",
                table: "DonHang",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiaChiGiaoHang",
                table: "DonHang");

            migrationBuilder.DropColumn(
                name: "SoDienThoai",
                table: "DonHang");

            migrationBuilder.DropColumn(
                name: "GhiChu",
                table: "DonHang");
        }
    }
} 