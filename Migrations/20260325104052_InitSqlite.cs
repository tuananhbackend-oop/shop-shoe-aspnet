using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LVTNWEBGIAYDEP.Migrations
{
    public partial class InitSqlite : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaiKhoan",
                columns: table => new
                {
                    taiKhoan = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    matKhau = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    hoTen = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    trangThai = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    phanQuyen = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaiKhoan", x => x.taiKhoan);
                });

            migrationBuilder.CreateTable(
                name: "ThuongHieu",
                columns: table => new
                {
                    MaTh = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    tenTH = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThuongHieu", x => x.MaTh);
                });

            migrationBuilder.CreateTable(
                name: "DonHang",
                columns: table => new
                {
                    iDDonHang = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    soDienThoai = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    diaChi = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    taiKhoan = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    thanhToan = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    trangThai = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    noiDungHuy = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    ngayLapDon = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonHang", x => x.iDDonHang);
                    table.ForeignKey(
                        name: "FK_DonHang_TaiKhoan",
                        column: x => x.taiKhoan,
                        principalTable: "TaiKhoan",
                        principalColumn: "taiKhoan");
                });

            migrationBuilder.CreateTable(
                name: "LienHe",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    noiDung = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    taiKhoan = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    trangThai = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    noiDungPhanHoi = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LienHe", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LienHe_TaiKhoan",
                        column: x => x.taiKhoan,
                        principalTable: "TaiKhoan",
                        principalColumn: "taiKhoan");
                });

            migrationBuilder.CreateTable(
                name: "LoaiSanPham",
                columns: table => new
                {
                    maLoai = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    tenLoai = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    maTH = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoaiSanPham", x => x.maLoai);
                    table.ForeignKey(
                        name: "FK_LoaiSanPham_ThuongHieu",
                        column: x => x.maTH,
                        principalTable: "ThuongHieu",
                        principalColumn: "MaTh");
                });

            migrationBuilder.CreateTable(
                name: "SanPham",
                columns: table => new
                {
                    maSP = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    tenSP = table.Column<string>(type: "TEXT", maxLength: 250, nullable: true),
                    hinhAnh = table.Column<string>(type: "TEXT", maxLength: 250, nullable: true),
                    giaBan = table.Column<int>(type: "INTEGER", nullable: true),
                    trangThai = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    moTa = table.Column<string>(type: "TEXT", maxLength: 4000, nullable: true),
                    hangTonKho = table.Column<int>(type: "INTEGER", nullable: true),
                    kichCo = table.Column<int>(type: "INTEGER", nullable: true),
                    maLoai = table.Column<int>(type: "INTEGER", nullable: false),
                    NgayThemSP = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SanPham_1", x => x.maSP);
                    table.ForeignKey(
                        name: "FK_SanPham_LoaiSanPham",
                        column: x => x.maLoai,
                        principalTable: "LoaiSanPham",
                        principalColumn: "maLoai");
                });

            migrationBuilder.CreateTable(
                name: "ChiTietDonHang",
                columns: table => new
                {
                    maSP = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    iDDonHang = table.Column<int>(type: "INTEGER", nullable: false),
                    soLuong = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietDonHang", x => new { x.maSP, x.iDDonHang });
                    table.ForeignKey(
                        name: "FK_ChiTietDonHang_DonHang",
                        column: x => x.iDDonHang,
                        principalTable: "DonHang",
                        principalColumn: "iDDonHang");
                    table.ForeignKey(
                        name: "FK_ChiTietDonHang_SanPham",
                        column: x => x.maSP,
                        principalTable: "SanPham",
                        principalColumn: "maSP");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDonHang_iDDonHang",
                table: "ChiTietDonHang",
                column: "iDDonHang");

            migrationBuilder.CreateIndex(
                name: "IX_DonHang_taiKhoan",
                table: "DonHang",
                column: "taiKhoan");

            migrationBuilder.CreateIndex(
                name: "IX_LienHe_taiKhoan",
                table: "LienHe",
                column: "taiKhoan");

            migrationBuilder.CreateIndex(
                name: "IX_LoaiSanPham_maTH",
                table: "LoaiSanPham",
                column: "maTH");

            migrationBuilder.CreateIndex(
                name: "IX_SanPham_maLoai",
                table: "SanPham",
                column: "maLoai");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiTietDonHang");

            migrationBuilder.DropTable(
                name: "LienHe");

            migrationBuilder.DropTable(
                name: "DonHang");

            migrationBuilder.DropTable(
                name: "SanPham");

            migrationBuilder.DropTable(
                name: "TaiKhoan");

            migrationBuilder.DropTable(
                name: "LoaiSanPham");

            migrationBuilder.DropTable(
                name: "ThuongHieu");
        }
    }
}
