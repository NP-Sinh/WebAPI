using System;
using System.Collections.Generic;

namespace WebAPI.Models;

public partial class GioHang
{
    public int MaGioHang { get; set; }
    public int MaNguoiDung { get; set; }
    public int MaMonAn { get; set; }
    public int SoLuong { get; set; }
    public DateTime NgayThem { get; set; }

    public virtual NguoiDung MaNguoiDungNavigation { get; set; }
    public virtual MonAn MaMonAnNavigation { get; set; }
}
