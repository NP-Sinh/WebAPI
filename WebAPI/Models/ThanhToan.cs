using System;
using System.Collections.Generic;

namespace WebAPI.Models;

public partial class ThanhToan
{
    public int MaThanhToan { get; set; }

    public int MaDonHang { get; set; }

    public DateTime? NgayThanhToan { get; set; }

    public decimal SoTien { get; set; }

    public string PhuongThucThanhToan { get; set; } = null!;

    public string TrangThai { get; set; } = null!;

    public virtual DonHang MaDonHangNavigation { get; set; } = null!;
}
