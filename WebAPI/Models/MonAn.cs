using System;
using System.Collections.Generic;

namespace WebAPI.Models;

public partial class MonAn
{
    public int MaMonAn { get; set; }

    public int MaNhaHang { get; set; }

    public string TenMonAn { get; set; } = null!;

    public string? MoTa { get; set; }

    public decimal Gia { get; set; }

    public string? UrlhinhAnh { get; set; }

    public DateTime? NgayTao { get; set; }

    public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();
    public virtual ICollection<GioHang> GioHangs { get; set; } = new List<GioHang>();
    public virtual NhaHang MaNhaHangNavigation { get; set; } = null!;
}
