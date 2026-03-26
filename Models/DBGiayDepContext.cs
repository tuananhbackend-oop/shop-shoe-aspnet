using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace LVTNWEBGIAYDEP.Models
{
    public partial class DBGiayDepContext : DbContext
    {
        public DBGiayDepContext()
        {
        }

        public DBGiayDepContext(DbContextOptions<DBGiayDepContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; } = null!;
        public virtual DbSet<DonHang> DonHangs { get; set; } = null!;
        public virtual DbSet<LienHe> LienHes { get; set; } = null!;
        public virtual DbSet<LoaiSanPham> LoaiSanPhams { get; set; } = null!;
      
        public virtual DbSet<SanPham> SanPhams { get; set; } = null!;
        public virtual DbSet<TaiKhoan> TaiKhoans { get; set; } = null!;
        public virtual DbSet<ThuongHieu> ThuongHieus { get; set; } = null!;

       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChiTietDonHang>(entity =>
            {
                entity.HasKey(e => new { e.MaSp, e.iDDonHang });

                entity.ToTable("ChiTietDonHang");

              

                entity.Property(e => e.MaSp)
                    .HasMaxLength(50)
                    .HasColumnName("maSP");

                entity.Property(e => e.iDDonHang)  // Đảm bảo rằng "soDienThoai" không phải là iDDonHang
                 .HasColumnName("iDDonHang");

                entity.Property(e => e.SoLuong).HasColumnName("soLuong");

                entity.HasOne(d => d.MaSpNavigation)
                    .WithMany(p => p.ChiTietDonHangs)
                    .HasForeignKey(d => d.MaSp)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ChiTietDonHang_SanPham");

                entity.HasOne(d => d.iDDonHangNavigation)  // Chú ý: Thay SoDienThoai bằng iDDonHang
         .WithMany(p => p.ChiTietDonHangs)
         .HasForeignKey(d => d.iDDonHang)  // Khóa ngoại tham chiếu đến iDDonHang
         .OnDelete(DeleteBehavior.ClientSetNull)
         .HasConstraintName("FK_ChiTietDonHang_DonHang");
            });

            modelBuilder.Entity<DonHang>(entity =>
            {
                // Đặt iDDonHang làm khóa chính
                entity.HasKey(e => e.iDDonHang);

                entity.ToTable("DonHang");

                // Cấu hình các thuộc tính
                entity.Property(e => e.SoDienThoai)
                    .HasMaxLength(50)
                    .HasColumnName("soDienThoai");

                entity.Property(e => e.DiaChi)
                    .HasMaxLength(50)
                    .HasColumnName("diaChi");

                entity.Property(e => e.TaiKhoan)
                    .HasMaxLength(50)
                    .HasColumnName("taiKhoan");

                entity.HasOne(d => d.TaiKhoanNavigation)
                    .WithMany(p => p.DonHangs)
                    .HasForeignKey(d => d.TaiKhoan)
                    .HasConstraintName("FK_DonHang_TaiKhoan");

                entity.Property(e => e.thanhToan)
                    .HasMaxLength(50)
                    .HasColumnName("thanhToan");

                entity.Property(e => e.trangThai)
                    .HasMaxLength(50)
                    .HasColumnName("trangThai");

                entity.Property(e => e.noiDungHuy)
                    .HasMaxLength(50)
                    .HasColumnName("noiDungHuy");

                entity.Property(e => e.ngayLapDon)
                    .HasColumnType("datetime")
                    .HasColumnName("ngayLapDon");
            });


            modelBuilder.Entity<LienHe>(entity =>
            {
                entity.ToTable("LienHe");

                // Đảm bảo 'Id' là khóa chính và tự động tăng
                entity.HasKey(e => e.Id);  // Khóa chính là 'Id'
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();  // Cấu hình để giá trị tự động tăng

                entity.Property(e => e.NoiDung)
                    .HasMaxLength(500)
                    .HasColumnName("noiDung");

                entity.Property(e => e.TaiKhoan)
                    .HasMaxLength(50)
                    .HasColumnName("taiKhoan");

                entity.Property(e => e.trangThai)
                    .HasMaxLength(50)
                    .HasColumnName("trangThai");

                entity.Property(e => e.noiDungPhanHoi)
                    .HasMaxLength(500)
                    .HasColumnName("noiDungPhanHoi");

                entity.HasOne(d => d.TaiKhoanNavigation)
                    .WithMany(p => p.LienHes)
                    .HasForeignKey(d => d.TaiKhoan)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LienHe_TaiKhoan");
            });


            modelBuilder.Entity<LoaiSanPham>(entity =>
            {
                entity.HasKey(e => e.MaLoai); // Khóa chính

                entity.ToTable("LoaiSanPham");

                entity.Property(e => e.MaLoai)
                    .ValueGeneratedOnAdd() // Tự động tăng
                    .HasColumnName("maLoai");

                entity.Property(e => e.MaTh)
                    .HasColumnName("maTH");

                entity.Property(e => e.TenLoai)
                    .HasMaxLength(50)
                    .HasColumnName("tenLoai");

                entity.HasOne(d => d.MaThNavigation)
                    .WithMany(p => p.LoaiSanPhams)
                    .HasForeignKey(d => d.MaTh)
                    .HasConstraintName("FK_LoaiSanPham_ThuongHieu");
            });


            modelBuilder.Entity<SanPham>(entity =>
            {
                entity.HasKey(e => e.MaSp)
                    .HasName("PK_SanPham_1");

                entity.ToTable("SanPham");

                entity.Property(e => e.MaSp)
                    .HasMaxLength(50)
                    .HasColumnName("maSP");

                entity.Property(e => e.GiaBan).HasColumnName("giaBan");

                entity.Property(e => e.HangTonKho).HasColumnName("hangTonKho");

                entity.Property(e => e.HinhAnh)
                    .HasMaxLength(250)
                    .HasColumnName("hinhAnh");

                entity.Property(e => e.KichCo).HasColumnName("kichCo");

                entity.Property(e => e.MaLoai) // Không sử dụng HasMaxLength
               .HasColumnName("maLoai");

                entity.Property(e => e.MoTa)
                    .HasMaxLength(4000)
                    .HasColumnName("moTa");

                entity.Property(e => e.TenSp)
                    .HasMaxLength(250)
                    .HasColumnName("tenSP");

                entity.Property(e => e.TrangThai)
                    .HasMaxLength(50)
                    .HasColumnName("trangThai");
                entity.Property(e => e.NgayThemSP)
                     .HasColumnType("datetime")
                      .HasColumnName("NgayThemSP");

                entity.HasOne(d => d.MaLoaiNavigation)
                    .WithMany(p => p.SanPhams)
                    .HasForeignKey(d => d.MaLoai)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SanPham_LoaiSanPham");

               
            });

            modelBuilder.Entity<TaiKhoan>(entity =>
            {
                entity.HasKey(e => e.TaiKhoan1);

                entity.ToTable("TaiKhoan");

                entity.Property(e => e.TaiKhoan1)
                    .HasMaxLength(100)
                    .HasColumnName("taiKhoan");

                entity.Property(e => e.HoTen)
                    .HasMaxLength(100)
                    .HasColumnName("hoTen");

                entity.Property(e => e.MatKhau)
                    .HasMaxLength(100)
                    .HasColumnName("matKhau");
				entity.Property(e => e.trangThai)
				   .HasMaxLength(50)
				   .HasColumnName("trangThai");
				entity.Property(e => e.phanQuyen)
				   .HasMaxLength(50)
				   .HasColumnName("phanQuyen");
			});

            modelBuilder.Entity<ThuongHieu>(entity =>
            {
                entity.HasKey(e => e.MaTh); // Đặt maTH làm khóa chính

                entity.ToTable("ThuongHieu");

                entity.Property(e => e.MaTh)
         .ValueGeneratedOnAdd() // Điều này sẽ yêu cầu EF tự động sinh giá trị cho maTH khi thêm bản ghi mới
         .HasColumnName("MaTh");

                entity.Property(e => e.TenTh)
                    .HasMaxLength(50)
                    .HasColumnName("tenTH");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
