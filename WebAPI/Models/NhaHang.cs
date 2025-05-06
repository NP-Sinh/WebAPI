using System;
using System.Collections.Generic;

namespace WebAPI.Models;

public partial class NhaHang
{
    public int MaNhaHang { get; set; }

    public string TenNhaHang { get; set; } = null!;

    public string? DiaChi { get; set; }

    public string? SoDienThoai { get; set; }

    public string? MoTa { get; set; }

    public DateTime? NgayTao { get; set; }

    public virtual ICollection<DanhGium> DanhGia { get; set; } = new List<DanhGium>();

    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();

    public virtual ICollection<MonAn> MonAns { get; set; } = new List<MonAn>();
}
