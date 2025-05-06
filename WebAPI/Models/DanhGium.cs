using System;
using System.Collections.Generic;

namespace WebAPI.Models;

public partial class DanhGium
{
    public int MaDanhGia { get; set; }

    public int MaNguoiDung { get; set; }

    public int MaNhaHang { get; set; }

    public int DiemDanhGia { get; set; }

    public string? BinhLuan { get; set; }

    public DateTime? NgayTao { get; set; }

    public virtual NguoiDung MaNguoiDungNavigation { get; set; } = null!;

    public virtual NhaHang MaNhaHangNavigation { get; set; } = null!;
}
