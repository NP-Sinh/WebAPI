using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models;
using Newtonsoft.Json.Linq;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GioHangController : ControllerBase
    {
        private readonly FoodOrderDBContext _context;

        public GioHangController(FoodOrderDBContext context)
        {
            _context = context;
        }

        // GET: api/GioHang
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GioHang>>> GetGioHangs()
        {
            return await _context.GioHangs.ToListAsync();
        }

        // GET: api/GioHang/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GioHang>> GetGioHang(int id)
        {
            var gioHang = await _context.GioHangs.FindAsync(id);

            if (gioHang == null)
            {
                return NotFound();
            }

            return gioHang;
        }

        // PUT: api/GioHang/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGioHang(int id, GioHang gioHang)
        {
            if (id != gioHang.MaGioHang)
            {
                return BadRequest();
            }

            _context.Entry(gioHang).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GioHangExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/GioHang
        [HttpPost]
        public async Task<ActionResult<GioHang>> PostGioHang(GioHangDTO gioHangDTO)
        {
            try
            {
                // Chuyển từ DTO sang entity
                var gioHang = new GioHang
                {
                    MaNguoiDung = gioHangDTO.MaNguoiDung,
                    MaMonAn = gioHangDTO.MaMonAn,
                    SoLuong = gioHangDTO.SoLuong,
                    NgayThem = gioHangDTO.NgayThem
                };

                // Kiểm tra xem món ăn đã có trong giỏ hàng của người dùng chưa
                var existingItem = await _context.GioHangs
                    .FirstOrDefaultAsync(g => g.MaNguoiDung == gioHang.MaNguoiDung && g.MaMonAn == gioHang.MaMonAn);

                if (existingItem != null)
                {
                    // Nếu món ăn đã tồn tại trong giỏ hàng, cập nhật số lượng
                    existingItem.SoLuong += gioHang.SoLuong;
                    await _context.SaveChangesAsync();
                    return CreatedAtAction("GetGioHang", new { id = existingItem.MaGioHang }, existingItem);
                }

                // Nếu là món ăn mới, thêm vào giỏ hàng
                _context.GioHangs.Add(gioHang);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetGioHang", new { id = gioHang.MaGioHang }, gioHang);
            }
            catch (Exception ex)
            {
                // In thông báo lỗi chi tiết
                return BadRequest(new { message = "Có lỗi khi thêm giỏ hàng", details = ex.Message });
            }
        }

        // DELETE: api/GioHang/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGioHang(int id)
        {
            var gioHang = await _context.GioHangs.FindAsync(id);
            if (gioHang == null)
            {
                return NotFound();
            }

            _context.GioHangs.Remove(gioHang);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/GioHang/nguoidung/5
        [HttpGet("nguoidung/{maNguoiDung}")]
        public async Task<ActionResult<IEnumerable<GioHang>>> GetGioHangByNguoiDung(int maNguoiDung)
        {
            var gioHangs = await _context.GioHangs
                .Where(g => g.MaNguoiDung == maNguoiDung)
                .Include(g => g.MaMonAnNavigation)
                .ToListAsync();

            if (gioHangs == null || !gioHangs.Any())
            {
                return new List<GioHang>();
            }

            return gioHangs;
        }

        private bool GioHangExists(int id)
        {
            return _context.GioHangs.Any(e => e.MaGioHang == id);
        }
    }
    // Lớp DTO để nhận dữ liệu từ client
    public class GioHangDTO
    {
        public int MaNguoiDung { get; set; }
        public int MaMonAn { get; set; }
        public int SoLuong { get; set; }
        public DateTime NgayThem { get; set; }
    }
}
