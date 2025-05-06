using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddGioHangTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NguoiDung",
                columns: table => new
                {
                    MaNguoiDung = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenDangNhap = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MatKhau = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    HoTen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SoDienThoai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    NgayTao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__NguoiDun__C539D76215A60F46", x => x.MaNguoiDung);
                });

            migrationBuilder.CreateTable(
                name: "NhaHang",
                columns: table => new
                {
                    MaNhaHang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenNhaHang = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DiaChi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SoDienThoai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    NgayTao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__NhaHang__7CE4848A27A5C5BB", x => x.MaNhaHang);
                });

            migrationBuilder.CreateTable(
                name: "DanhGia",
                columns: table => new
                {
                    MaDanhGia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNguoiDung = table.Column<int>(type: "int", nullable: false),
                    MaNhaHang = table.Column<int>(type: "int", nullable: false),
                    DiemDanhGia = table.Column<int>(type: "int", nullable: false),
                    BinhLuan = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    NgayTao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DanhGia__AA9515BF478FB2A7", x => x.MaDanhGia);
                    table.ForeignKey(
                        name: "FK__DanhGia__MaNguoi__5165187F",
                        column: x => x.MaNguoiDung,
                        principalTable: "NguoiDung",
                        principalColumn: "MaNguoiDung");
                    table.ForeignKey(
                        name: "FK__DanhGia__MaNhaHa__52593CB8",
                        column: x => x.MaNhaHang,
                        principalTable: "NhaHang",
                        principalColumn: "MaNhaHang");
                });

            migrationBuilder.CreateTable(
                name: "DonHang",
                columns: table => new
                {
                    MaDonHang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNguoiDung = table.Column<int>(type: "int", nullable: false),
                    MaNhaHang = table.Column<int>(type: "int", nullable: false),
                    NgayDatHang = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    TongTien = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DonHang__129584AD01657466", x => x.MaDonHang);
                    table.ForeignKey(
                        name: "FK__DonHang__MaNguoi__440B1D61",
                        column: x => x.MaNguoiDung,
                        principalTable: "NguoiDung",
                        principalColumn: "MaNguoiDung");
                    table.ForeignKey(
                        name: "FK__DonHang__MaNhaHa__44FF419A",
                        column: x => x.MaNhaHang,
                        principalTable: "NhaHang",
                        principalColumn: "MaNhaHang");
                });

            migrationBuilder.CreateTable(
                name: "MonAn",
                columns: table => new
                {
                    MaMonAn = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNhaHang = table.Column<int>(type: "int", nullable: false),
                    TenMonAn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Gia = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    URLHinhAnh = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    NgayTao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__MonAn__B1171625EE1AB43B", x => x.MaMonAn);
                    table.ForeignKey(
                        name: "FK__MonAn__MaNhaHang__403A8C7D",
                        column: x => x.MaNhaHang,
                        principalTable: "NhaHang",
                        principalColumn: "MaNhaHang");
                });

            migrationBuilder.CreateTable(
                name: "ThanhToan",
                columns: table => new
                {
                    MaThanhToan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaDonHang = table.Column<int>(type: "int", nullable: false),
                    NgayThanhToan = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    SoTien = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    PhuongThucThanhToan = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ThanhToa__D4B258447A4ADE85", x => x.MaThanhToan);
                    table.ForeignKey(
                        name: "FK__ThanhToan__MaDon__4CA06362",
                        column: x => x.MaDonHang,
                        principalTable: "DonHang",
                        principalColumn: "MaDonHang");
                });

            migrationBuilder.CreateTable(
                name: "ChiTietDonHang",
                columns: table => new
                {
                    MaChiTietDonHang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaDonHang = table.Column<int>(type: "int", nullable: false),
                    MaMonAn = table.Column<int>(type: "int", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    Gia = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ChiTietD__4B0B45DD7B737F2C", x => x.MaChiTietDonHang);
                    table.ForeignKey(
                        name: "FK__ChiTietDo__MaDon__47DBAE45",
                        column: x => x.MaDonHang,
                        principalTable: "DonHang",
                        principalColumn: "MaDonHang");
                    table.ForeignKey(
                        name: "FK__ChiTietDo__MaMon__48CFD27E",
                        column: x => x.MaMonAn,
                        principalTable: "MonAn",
                        principalColumn: "MaMonAn");
                });

            migrationBuilder.CreateTable(
                name: "GioHang",
                columns: table => new
                {
                    MaGioHang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNguoiDung = table.Column<int>(type: "int", nullable: false),
                    MaMonAn = table.Column<int>(type: "int", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    NgayThem = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__GioHang", x => x.MaGioHang);
                    table.ForeignKey(
                        name: "FK_GioHang_MonAn",
                        column: x => x.MaMonAn,
                        principalTable: "MonAn",
                        principalColumn: "MaMonAn");
                    table.ForeignKey(
                        name: "FK_GioHang_NguoiDung",
                        column: x => x.MaNguoiDung,
                        principalTable: "NguoiDung",
                        principalColumn: "MaNguoiDung");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDonHang_MaDonHang",
                table: "ChiTietDonHang",
                column: "MaDonHang");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDonHang_MaMonAn",
                table: "ChiTietDonHang",
                column: "MaMonAn");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGia_MaNguoiDung",
                table: "DanhGia",
                column: "MaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGia_MaNhaHang",
                table: "DanhGia",
                column: "MaNhaHang");

            migrationBuilder.CreateIndex(
                name: "IX_DonHang_MaNguoiDung",
                table: "DonHang",
                column: "MaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_DonHang_MaNhaHang",
                table: "DonHang",
                column: "MaNhaHang");

            migrationBuilder.CreateIndex(
                name: "IX_GioHang_MaMonAn",
                table: "GioHang",
                column: "MaMonAn");

            migrationBuilder.CreateIndex(
                name: "IX_GioHang_MaNguoiDung",
                table: "GioHang",
                column: "MaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_MonAn_MaNhaHang",
                table: "MonAn",
                column: "MaNhaHang");

            migrationBuilder.CreateIndex(
                name: "UQ__NguoiDun__55F68FC00384FA69",
                table: "NguoiDung",
                column: "TenDangNhap",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__NguoiDun__A9D105343961A706",
                table: "NguoiDung",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ThanhToan_MaDonHang",
                table: "ThanhToan",
                column: "MaDonHang");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiTietDonHang");

            migrationBuilder.DropTable(
                name: "DanhGia");

            migrationBuilder.DropTable(
                name: "GioHang");

            migrationBuilder.DropTable(
                name: "ThanhToan");

            migrationBuilder.DropTable(
                name: "MonAn");

            migrationBuilder.DropTable(
                name: "DonHang");

            migrationBuilder.DropTable(
                name: "NguoiDung");

            migrationBuilder.DropTable(
                name: "NhaHang");
        }
    }
}
