using System;
using System.Collections.Generic;

namespace WebAPI.Models;

public partial class DonHang
{
    public int MaDonHang { get; set; }

    public int MaNguoiDung { get; set; }

    public int MaNhaHang { get; set; }

    public DateTime? NgayDatHang { get; set; }

    public decimal TongTien { get; set; }

    public string TrangThai { get; set; } = null!;

    public string? DiaChiGiaoHang { get; set; }

    public string? SoDienThoai { get; set; }

    public string? GhiChu { get; set; }

    public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();

    public virtual NguoiDung MaNguoiDungNavigation { get; set; } = null!;

    public virtual NhaHang MaNhaHangNavigation { get; set; } = null!;

    public virtual ICollection<ThanhToan> ThanhToans { get; set; } = new List<ThanhToan>();
}
