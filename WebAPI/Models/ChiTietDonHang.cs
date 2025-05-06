using System;
using System.Collections.Generic;

namespace WebAPI.Models;

public partial class ChiTietDonHang
{
    public int MaChiTietDonHang { get; set; }

    public int MaDonHang { get; set; }

    public int MaMonAn { get; set; }

    public int SoLuong { get; set; }

    public decimal Gia { get; set; }

    public virtual DonHang MaDonHangNavigation { get; set; } = null!;

    public virtual MonAn MaMonAnNavigation { get; set; } = null!;
}
