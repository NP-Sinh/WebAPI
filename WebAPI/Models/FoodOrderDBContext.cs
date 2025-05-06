using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Models;

public partial class FoodOrderDBContext : DbContext
{
    public FoodOrderDBContext()
    {
    }

    public FoodOrderDBContext(DbContextOptions<FoodOrderDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }

    public virtual DbSet<DanhGium> DanhGia { get; set; }

    public virtual DbSet<DonHang> DonHangs { get; set; }

    public virtual DbSet<MonAn> MonAns { get; set; }

    public virtual DbSet<NguoiDung> NguoiDungs { get; set; }

    public virtual DbSet<NhaHang> NhaHangs { get; set; }

    public virtual DbSet<ThanhToan> ThanhToans { get; set; }
    public virtual DbSet<GioHang> GioHangs { get; set; }
    public virtual DbSet<ChatHistory> ChatHistories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:Connection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChiTietDonHang>(entity =>
        {
            entity.HasKey(e => e.MaChiTietDonHang).HasName("PK__ChiTietD__4B0B45DD7B737F2C");

            entity.ToTable("ChiTietDonHang");

            entity.Property(e => e.Gia).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.MaDonHangNavigation).WithMany(p => p.ChiTietDonHangs)
                .HasForeignKey(d => d.MaDonHang)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietDo__MaDon__47DBAE45");

            entity.HasOne(d => d.MaMonAnNavigation).WithMany(p => p.ChiTietDonHangs)
                .HasForeignKey(d => d.MaMonAn)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietDo__MaMon__48CFD27E");
        });

        modelBuilder.Entity<DanhGium>(entity =>
        {
            entity.HasKey(e => e.MaDanhGia).HasName("PK__DanhGia__AA9515BF478FB2A7");

            entity.Property(e => e.BinhLuan).HasMaxLength(500);
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.DanhGia)
                .HasForeignKey(d => d.MaNguoiDung)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DanhGia__MaNguoi__5165187F");

            entity.HasOne(d => d.MaNhaHangNavigation).WithMany(p => p.DanhGia)
                .HasForeignKey(d => d.MaNhaHang)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DanhGia__MaNhaHa__52593CB8");
        });

        modelBuilder.Entity<DonHang>(entity =>
        {
            entity.HasKey(e => e.MaDonHang).HasName("PK__DonHang__129584AD01657466");

            entity.ToTable("DonHang");

            entity.Property(e => e.NgayDatHang)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TongTien).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TrangThai).HasMaxLength(50);
            entity.Property(e => e.DiaChiGiaoHang).HasMaxLength(200);
            entity.Property(e => e.SoDienThoai).HasMaxLength(20);  
            entity.Property(e => e.GhiChu).HasMaxLength(500);

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.DonHangs)
                .HasForeignKey(d => d.MaNguoiDung)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DonHang__MaNguoi__440B1D61");

            entity.HasOne(d => d.MaNhaHangNavigation).WithMany(p => p.DonHangs)
                .HasForeignKey(d => d.MaNhaHang)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DonHang__MaNhaHa__44FF419A");
        });

        modelBuilder.Entity<MonAn>(entity =>
        {
            entity.HasKey(e => e.MaMonAn).HasName("PK__MonAn__B1171625EE1AB43B");

            entity.ToTable("MonAn");

            entity.Property(e => e.Gia).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.MoTa).HasMaxLength(500);
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TenMonAn).HasMaxLength(100);
            entity.Property(e => e.UrlhinhAnh)
                .HasMaxLength(255)
                .HasColumnName("URLHinhAnh");

            entity.HasOne(d => d.MaNhaHangNavigation).WithMany(p => p.MonAns)
                .HasForeignKey(d => d.MaNhaHang)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MonAn__MaNhaHang__403A8C7D");
        });

        modelBuilder.Entity<NguoiDung>(entity =>
        {
            entity.HasKey(e => e.MaNguoiDung).HasName("PK__NguoiDun__C539D76215A60F46");

            entity.ToTable("NguoiDung");

            entity.HasIndex(e => e.TenDangNhap, "UQ__NguoiDun__55F68FC00384FA69").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__NguoiDun__A9D105343961A706").IsUnique();

            entity.Property(e => e.DiaChi).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.HoTen).HasMaxLength(100);
            entity.Property(e => e.MatKhau).HasMaxLength(255);
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.SoDienThoai).HasMaxLength(20);
            entity.Property(e => e.TenDangNhap).HasMaxLength(50);
        });

        modelBuilder.Entity<NhaHang>(entity =>
        {
            entity.HasKey(e => e.MaNhaHang).HasName("PK__NhaHang__7CE4848A27A5C5BB");

            entity.ToTable("NhaHang");

            entity.Property(e => e.DiaChi).HasMaxLength(255);
            entity.Property(e => e.MoTa).HasMaxLength(500);
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.SoDienThoai).HasMaxLength(20);
            entity.Property(e => e.TenNhaHang).HasMaxLength(100);
        });

        modelBuilder.Entity<ThanhToan>(entity =>
        {
            entity.HasKey(e => e.MaThanhToan).HasName("PK__ThanhToa__D4B258447A4ADE85");

            entity.ToTable("ThanhToan");

            entity.Property(e => e.NgayThanhToan)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PhuongThucThanhToan).HasMaxLength(50);
            entity.Property(e => e.SoTien).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TrangThai).HasMaxLength(50);

            entity.HasOne(d => d.MaDonHangNavigation).WithMany(p => p.ThanhToans)
                .HasForeignKey(d => d.MaDonHang)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ThanhToan__MaDon__4CA06362");
        });
        modelBuilder.Entity<GioHang>(entity =>
        {
            entity.HasKey(e => e.MaGioHang).HasName("PK__GioHang");

            entity.ToTable("GioHang");

            entity.Property(e => e.NgayThem)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.Property(e => e.SoLuong)
                .IsRequired();

            entity.HasOne(d => d.MaNguoiDungNavigation)
                .WithMany(p => p.GioHangs)
                .HasForeignKey(d => d.MaNguoiDung)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GioHang_NguoiDung");

            entity.HasOne(d => d.MaMonAnNavigation)
                .WithMany(p => p.GioHangs)
                .HasForeignKey(d => d.MaMonAn)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GioHang_MonAn");
        });

        modelBuilder.Entity<ChatHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ChatHistory__3214EC07");

            entity.ToTable("ChatHistory");

            entity.Property(e => e.UserMessage).HasMaxLength(500);
            entity.Property(e => e.BotResponse).HasMaxLength(2000);
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.NguoiDung)
                .WithMany()
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ChatHistory_NguoiDung");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
