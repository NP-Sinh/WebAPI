using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NhaHangController : ControllerBase
    {
        private readonly FoodOrderDBContext _context;
        private readonly ILogger<NhaHangController> _logger;

        public NhaHangController(FoodOrderDBContext context, ILogger<NhaHangController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/NhaHang
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NhaHang>>> GetNhaHangs()
        {
            try 
            {
                return await _context.NhaHangs
                    .Select(n => new NhaHang 
                    {
                        MaNhaHang = n.MaNhaHang,
                        TenNhaHang = n.TenNhaHang,
                        DiaChi = n.DiaChi,
                        SoDienThoai = n.SoDienThoai,
                        MoTa = n.MoTa,
                        NgayTao = n.NgayTao
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Đã xảy ra lỗi khi lấy danh sách nhà hàng");
            }
        }

        // GET: api/NhaHang/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NhaHang>> GetNhaHang(int id)
        {
            try
            {
                var nhaHang = await _context.NhaHangs
                    .Where(n => n.MaNhaHang == id)
                    .Select(n => new NhaHang
                    {
                        MaNhaHang = n.MaNhaHang,
                        TenNhaHang = n.TenNhaHang,
                        DiaChi = n.DiaChi,
                        SoDienThoai = n.SoDienThoai,
                        MoTa = n.MoTa,
                        NgayTao = n.NgayTao
                    })
                    .FirstOrDefaultAsync();

                if (nhaHang == null)
                {
                    return NotFound("Không tìm thấy nhà hàng");
                }

                return nhaHang;
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Đã xảy ra lỗi khi lấy thông tin nhà hàng");
            }
        }

        // GET: api/NhaHang/5/MonAn
        [HttpGet("{id}/MonAn")]
        public async Task<ActionResult<NhaHangWithMonAn>> GetNhaHangWithMonAn(int id)
        {
            try
            {
                var nhaHang = await _context.NhaHangs
                    .Where(n => n.MaNhaHang == id)
                    .Select(n => new NhaHangWithMonAn
                    {
                        MaNhaHang = n.MaNhaHang,
                        TenNhaHang = n.TenNhaHang,
                        DiaChi = n.DiaChi,
                        SoDienThoai = n.SoDienThoai,
                        MoTa = n.MoTa,
                        NgayTao = n.NgayTao,
                        MonAns = n.MonAns.Select(m => new MonAn
                        {
                            MaMonAn = m.MaMonAn,
                            MaNhaHang = m.MaNhaHang,
                            TenMonAn = m.TenMonAn,
                            MoTa = m.MoTa,
                            Gia = m.Gia,
                            UrlhinhAnh = m.UrlhinhAnh,
                            NgayTao = m.NgayTao
                        }).ToList()
                    })
                    .FirstOrDefaultAsync();

                if (nhaHang == null)
                {
                    return NotFound("Không tìm thấy nhà hàng");
                }

                return nhaHang;
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Đã xảy ra lỗi khi lấy thông tin nhà hàng và món ăn");
            }
        }

        // PUT: api/NhaHang/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNhaHang(int id, NhaHang nhaHang)
        {
            if (id != nhaHang.MaNhaHang)
            {
                return BadRequest("ID không khớp");
            }

            _context.Entry(nhaHang).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NhaHangExists(id))
                {
                    return NotFound("Không tìm thấy nhà hàng");
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Đã xảy ra lỗi khi cập nhật nhà hàng");
            }
        }

        // POST: api/NhaHang
        [HttpPost]
        public async Task<ActionResult<NhaHang>> PostNhaHang(NhaHang nhaHang)
        {
            try
            {
                _context.NhaHangs.Add(nhaHang);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetNhaHang", new { id = nhaHang.MaNhaHang }, nhaHang);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Đã xảy ra lỗi khi tạo nhà hàng mới");
            }
        }

        // DELETE: api/NhaHang/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNhaHang(int id)
        {
            try
            {
                var nhaHang = await _context.NhaHangs.FindAsync(id);
                if (nhaHang == null)
                {
                    return NotFound("Không tìm thấy nhà hàng");
                }

                _context.NhaHangs.Remove(nhaHang);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Đã xảy ra lỗi khi xóa nhà hàng");
            }
        }

        private bool NhaHangExists(int id)
        {
            return _context.NhaHangs.Any(e => e.MaNhaHang == id);
        }
    }

    public class NhaHangWithMonAn : NhaHang
    {
        public List<MonAn>? MonAns { get; set; }
    }
}
