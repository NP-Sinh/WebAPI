using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models;
using System.Text.Json;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonHangController : ControllerBase
    {
        private readonly FoodOrderDBContext _context;
        private readonly ILogger<DonHangController> _logger;

        public DonHangController(FoodOrderDBContext context, ILogger<DonHangController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/DonHang
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DonHang>>> GetDonHangs()
        {
            try
            {
                return await _context.DonHangs
                    .Include(d => d.ChiTietDonHangs)
                    .Include(d => d.MaNguoiDungNavigation)
                    .Include(d => d.MaNhaHangNavigation)
                    .OrderByDescending(d => d.NgayDatHang)
                    .ToListAsync();
            }
            catch (Exception)
            {
                return StatusCode(500, "Đã xảy ra lỗi khi lấy danh sách đơn hàng");
            }
        }

        // GET: api/DonHang/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DonHang>> GetDonHang(int id)
        {
            try
            {
                var donHang = await _context.DonHangs
                    .Include(d => d.ChiTietDonHangs)
                    .ThenInclude(c => c.MaMonAnNavigation)
                    .Include(d => d.ThanhToans)
                    .Include(d => d.MaNguoiDungNavigation)
                    .Include(d => d.MaNhaHangNavigation)
                    .FirstOrDefaultAsync(d => d.MaDonHang == id);

                if (donHang == null)
                {
                    return NotFound();
                }

                return donHang;
            }
            catch (Exception)
            {
                return StatusCode(500, "Đã xảy ra lỗi khi lấy thông tin đơn hàng");
            }
        }

        // GET: api/DonHang/nguoidung/5
        [HttpGet("nguoidung/{maNguoiDung}")]
        public async Task<ActionResult<IEnumerable<DonHang>>> GetDonHangByNguoiDung(int maNguoiDung)
        {
            try
            {
                return await _context.DonHangs
                    .Include(d => d.ChiTietDonHangs)
                    .ThenInclude(c => c.MaMonAnNavigation)
                    .Include(d => d.MaNguoiDungNavigation)
                    .Include(d => d.MaNhaHangNavigation)
                    .Where(d => d.MaNguoiDung == maNguoiDung)
                    .OrderByDescending(d => d.NgayDatHang)
                    .ToListAsync();
            }
            catch (Exception)
            {
                return StatusCode(500, "Đã xảy ra lỗi khi lấy đơn hàng");
            }
        }

        // GET: api/DonHang/nguoidung/5/trangthai/Đang xử lý
        [HttpGet("nguoidung/{maNguoiDung}/trangthai/{trangThai}")]
        public async Task<ActionResult<IEnumerable<DonHang>>> GetDonHangByTrangThai(int maNguoiDung, string trangThai)
        {
            try
            {
                // Kiểm tra trạng thái hợp lệ
                if (trangThai != "Đang xử lý" && trangThai != "Hoàn thành" && trangThai != "Đã hủy")
                {
                    return BadRequest("Trạng thái không hợp lệ. Các trạng thái hợp lệ: 'Đang xử lý', 'Hoàn thành', 'Đã hủy'");
                }

                return await _context.DonHangs
                    .Include(d => d.ChiTietDonHangs)
                    .ThenInclude(c => c.MaMonAnNavigation)
                    .Include(d => d.ThanhToans)
                    .Include(d => d.MaNguoiDungNavigation)
                    .Include(d => d.MaNhaHangNavigation)
                    .Where(d => d.MaNguoiDung == maNguoiDung && d.TrangThai == trangThai)
                    .OrderByDescending(d => d.NgayDatHang)
                    .ToListAsync();
            }
            catch (Exception)
            {
                return StatusCode(500, "Đã xảy ra lỗi khi lọc đơn hàng theo trạng thái");
            }
        }

        // PUT: api/DonHang/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDonHang(int id, DonHang donHang)
        {
            if (id != donHang.MaDonHang)
            {
                return BadRequest("ID không khớp");
            }

            _context.Entry(donHang).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "Đã xảy ra lỗi khi cập nhật đơn hàng");
            }
        }

        // PUT: api/DonHang/5/TrangThai/Hoàn thành
        [HttpPut("{id}/TrangThai/{trangThai}")]
        public async Task<IActionResult> UpdateTrangThaiDonHang(int id, string trangThai)
        {
            if (trangThai != "Đang xử lý" && trangThai != "Hoàn thành" && trangThai != "Đã hủy")
            {
                return BadRequest("Trạng thái không hợp lệ. Các trạng thái hợp lệ: 'Đang xử lý', 'Hoàn thành', 'Đã hủy'");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var donHang = await _context.DonHangs.FindAsync(id);
                if (donHang == null)
                {
                    return NotFound("Không tìm thấy đơn hàng");
                }

                // Cập nhật trạng thái đơn hàng
                donHang.TrangThai = trangThai;
                
                // Cập nhật trạng thái thanh toán nếu trạng thái đơn hàng là "Hoàn thành"
                if (trangThai == "Hoàn thành")
                {
                    var thanhToan = await _context.ThanhToans
                        .Where(t => t.MaDonHang == id)
                        .FirstOrDefaultAsync();
                    
                    if (thanhToan != null && thanhToan.TrangThai == "Chờ thanh toán")
                    {
                        thanhToan.TrangThai = "Đã thanh toán";
                        thanhToan.NgayThanhToan = DateTime.Now;
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return NoContent();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, "Đã xảy ra lỗi khi cập nhật trạng thái đơn hàng");
            }
        }

        // POST: api/DonHang
        [HttpPost]
        public async Task<ActionResult<DonHang>> PostDonHang(DonHangDto donHangDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Tạo đơn hàng mới
                var donHang = new DonHang
                {
                    MaNguoiDung = donHangDto.MaNguoiDung,
                    MaNhaHang = donHangDto.MaNhaHang,
                    NgayDatHang = donHangDto.NgayDatHang ?? DateTime.Now,
                    TongTien = Convert.ToDecimal(donHangDto.TongTien),
                    TrangThai = donHangDto.TrangThai ?? "Đang xử lý",
                    DiaChiGiaoHang = donHangDto.DiaChiGiaoHang,
                    SoDienThoai = donHangDto.SoDienThoai,
                    GhiChu = donHangDto.GhiChu
                };

                _context.DonHangs.Add(donHang);
                await _context.SaveChangesAsync();

                // Thêm chi tiết đơn hàng
                if (donHangDto.ChiTietDonHangs != null && donHangDto.ChiTietDonHangs.Any())
                {
                    foreach (var chiTiet in donHangDto.ChiTietDonHangs)
                    {
                        var chiTietDonHang = new ChiTietDonHang
                        {
                            MaDonHang = donHang.MaDonHang,
                            MaMonAn = chiTiet.MaMonAn,
                            SoLuong = chiTiet.SoLuong,
                            Gia = Convert.ToDecimal(chiTiet.Gia)
                        };
                        _context.ChiTietDonHangs.Add(chiTietDonHang);
                    }
                    await _context.SaveChangesAsync();
                }

                // Tạo bản ghi thanh toán
                if (!string.IsNullOrEmpty(donHangDto.PhuongThucThanhToan))
                {
                    var trangThaiThanhToan = donHangDto.PhuongThucThanhToan.ToLower() == "offline" 
                        ? "Chờ thanh toán" 
                        : "Đã thanh toán";
                    
                    var thanhToan = new ThanhToan
                    {
                        MaDonHang = donHang.MaDonHang,
                        NgayThanhToan = donHangDto.PhuongThucThanhToan.ToLower() == "offline" ? null : DateTime.Now,
                        SoTien = Convert.ToDecimal(donHangDto.TongTien),
                        PhuongThucThanhToan = donHangDto.PhuongThucThanhToan,
                        TrangThai = trangThaiThanhToan
                    };
                    _context.ThanhToans.Add(thanhToan);
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
                
                // Lấy đơn hàng đầy đủ để trả về
                var donHangResult = await _context.DonHangs
                    .Include(d => d.ChiTietDonHangs)
                    .ThenInclude(c => c.MaMonAnNavigation)
                    .Include(d => d.ThanhToans)
                    .FirstOrDefaultAsync(d => d.MaDonHang == donHang.MaDonHang);
                
                return CreatedAtAction("GetDonHang", new { id = donHang.MaDonHang }, donHangResult);
            }
            catch (DbUpdateException dbEx)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, $"Lỗi khi tạo đơn hàng: {dbEx.InnerException?.Message ?? dbEx.Message}");
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, "Lỗi khi tạo đơn hàng");
            }
        }

        // DELETE: api/DonHang/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDonHang(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Xóa các chi tiết đơn hàng trước
                var chiTietDonHangs = await _context.ChiTietDonHangs
                    .Where(c => c.MaDonHang == id).ToListAsync();
                
                if (chiTietDonHangs.Any())
                {
                    _context.ChiTietDonHangs.RemoveRange(chiTietDonHangs);
                    await _context.SaveChangesAsync();
                }

                // Xóa các thanh toán 
                var thanhToans = await _context.ThanhToans
                    .Where(t => t.MaDonHang == id).ToListAsync();
                
                if (thanhToans.Any())
                {
                    _context.ThanhToans.RemoveRange(thanhToans);
                    await _context.SaveChangesAsync();
                }

                // Xóa đơn hàng
                var donHang = await _context.DonHangs.FindAsync(id);
                if (donHang == null)
                {
                    return NotFound("Không tìm thấy đơn hàng");
                }

                _context.DonHangs.Remove(donHang);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return NoContent();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, "Đã xảy ra lỗi khi xóa đơn hàng");
            }
        }
    }

    // DTO cho việc tạo đơn hàng
    public class DonHangDto
    {
        public int MaNguoiDung { get; set; }
        public int MaNhaHang { get; set; }
        public DateTime? NgayDatHang { get; set; }
        public double TongTien { get; set; }
        public string? TrangThai { get; set; }
        public string? DiaChiGiaoHang { get; set; }
        public string? SoDienThoai { get; set; }
        public string? GhiChu { get; set; }
        public string? PhuongThucThanhToan { get; set; }
        public List<ChiTietDonHangDto>? ChiTietDonHangs { get; set; }
    }

    public class ChiTietDonHangDto
    {
        public int MaMonAn { get; set; }
        public int SoLuong { get; set; }
        public double Gia { get; set; }
    }
}
