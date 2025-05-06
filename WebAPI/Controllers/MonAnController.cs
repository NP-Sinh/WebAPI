using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models;
using Microsoft.Extensions.Logging;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonAnController : ControllerBase
    {
        private readonly FoodOrderDBContext _context;
        private readonly ILogger<MonAnController> _logger;

        public MonAnController(FoodOrderDBContext context, ILogger<MonAnController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/MonAn
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MonAn>>> GetMonAns()
        {
            try
            {
                var monAns = await _context.MonAns
                    .Include(m => m.MaNhaHangNavigation)
                    .Select(m => new MonAn
                    {
                        MaMonAn = m.MaMonAn,
                        MaNhaHang = m.MaNhaHang,
                        TenMonAn = m.TenMonAn,
                        MoTa = m.MoTa,
                        Gia = m.Gia,
                        UrlhinhAnh = m.UrlhinhAnh,
                        NgayTao = m.NgayTao,
                        MaNhaHangNavigation = new NhaHang
                        {
                            MaNhaHang = m.MaNhaHangNavigation.MaNhaHang,
                            TenNhaHang = m.MaNhaHangNavigation.TenNhaHang,
                            DiaChi = m.MaNhaHangNavigation.DiaChi,
                            SoDienThoai = m.MaNhaHangNavigation.SoDienThoai,
                            MoTa = m.MaNhaHangNavigation.MoTa,
                            NgayTao = m.MaNhaHangNavigation.NgayTao
                        }
                    })
                    .ToListAsync();

                return monAns;
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Đã xảy ra lỗi khi lấy danh sách món ăn");
            }
        }

        // GET: api/MonAn/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MonAn>> GetMonAn(int id)
        {
            try
            {
                var monAn = await _context.MonAns
                    .Include(m => m.MaNhaHangNavigation)
                    .FirstOrDefaultAsync(m => m.MaMonAn == id);

                if (monAn == null)
                {
                    return NotFound("Không tìm thấy món ăn");
                }

                // Tạo đối tượng món ăn mới không có circular references
                var result = new MonAn
                {
                    MaMonAn = monAn.MaMonAn,
                    MaNhaHang = monAn.MaNhaHang,
                    TenMonAn = monAn.TenMonAn,
                    MoTa = monAn.MoTa,
                    Gia = monAn.Gia,
                    UrlhinhAnh = monAn.UrlhinhAnh,
                    NgayTao = monAn.NgayTao,
                    MaNhaHangNavigation = new NhaHang
                    {
                        MaNhaHang = monAn.MaNhaHangNavigation.MaNhaHang,
                        TenNhaHang = monAn.MaNhaHangNavigation.TenNhaHang,
                        DiaChi = monAn.MaNhaHangNavigation.DiaChi,
                        SoDienThoai = monAn.MaNhaHangNavigation.SoDienThoai,
                        MoTa = monAn.MaNhaHangNavigation.MoTa,
                        NgayTao = monAn.MaNhaHangNavigation.NgayTao
                    }
                };

                return result;
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Đã xảy ra lỗi khi lấy thông tin món ăn");
            }
        }

        // GET: api/MonAn/NhaHang/5
        [HttpGet("NhaHang/{maNhaHang}")]
        public async Task<ActionResult<IEnumerable<MonAn>>> GetMonAnByNhaHang(int maNhaHang)
        {
            try
            {
                var monAns = await _context.MonAns
                    .Where(m => m.MaNhaHang == maNhaHang)
                    .Select(m => new MonAn
                    {
                        MaMonAn = m.MaMonAn,
                        MaNhaHang = m.MaNhaHang,
                        TenMonAn = m.TenMonAn,
                        MoTa = m.MoTa,
                        Gia = m.Gia,
                        UrlhinhAnh = m.UrlhinhAnh,
                        NgayTao = m.NgayTao
                    })
                    .ToListAsync();

                return monAns;
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Đã xảy ra lỗi khi lấy danh sách món ăn theo nhà hàng");
            }
        }

        // GET: api/MonAn/bestselling
        [HttpGet("bestselling")]
        public async Task<ActionResult<IEnumerable<MonAn>>> GetBestSellingFoods()
        {
            try
            {
                // Lấy món ăn bán chạy nhất dựa trên số lượng đặt hàng
                var bestSellingFoods = await _context.ChiTietDonHangs
                    .GroupBy(c => c.MaMonAn)
                    .Select(g => new { MaMonAn = g.Key, TotalQuantity = g.Sum(c => c.SoLuong) })
                    .OrderByDescending(x => x.TotalQuantity)
                    .Take(5)
                    .Join(_context.MonAns,
                        bestseller => bestseller.MaMonAn,
                        monan => monan.MaMonAn,
                        (bestseller, monan) => new MonAn
                        {
                            MaMonAn = monan.MaMonAn,
                            MaNhaHang = monan.MaNhaHang,
                            TenMonAn = monan.TenMonAn,
                            MoTa = monan.MoTa,
                            Gia = monan.Gia,
                            UrlhinhAnh = monan.UrlhinhAnh,
                            NgayTao = monan.NgayTao
                        })
                    .ToListAsync();

                if (bestSellingFoods == null || bestSellingFoods.Count == 0)
                {
                    return NotFound("Không tìm thấy món ăn bán chạy");
                }

                return bestSellingFoods;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting best selling foods");
                return StatusCode(500, "Đã xảy ra lỗi khi lấy món ăn bán chạy nhất");
            }
        }

        // GET: api/MonAn/mostexpensive
        [HttpGet("mostexpensive")]
        public async Task<ActionResult<IEnumerable<MonAn>>> GetMostExpensiveFoods()
        {
            try
            {
                var mostExpensiveFoods = await _context.MonAns
                    .OrderByDescending(m => m.Gia)
                    .Take(5)
                    .Select(m => new MonAn
                    {
                        MaMonAn = m.MaMonAn,
                        MaNhaHang = m.MaNhaHang,
                        TenMonAn = m.TenMonAn,
                        MoTa = m.MoTa,
                        Gia = m.Gia,
                        UrlhinhAnh = m.UrlhinhAnh,
                        NgayTao = m.NgayTao
                    })
                    .ToListAsync();

                if (mostExpensiveFoods == null || mostExpensiveFoods.Count == 0)
                {
                    return NotFound("Không tìm thấy món ăn");
                }

                return mostExpensiveFoods;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting most expensive foods");
                return StatusCode(500, "Đã xảy ra lỗi khi lấy món ăn đắt nhất");
            }
        }

        // GET: api/MonAn/cheapest
        [HttpGet("cheapest")]
        public async Task<ActionResult<IEnumerable<MonAn>>> GetCheapestFoods()
        {
            try
            {
                var cheapestFoods = await _context.MonAns
                    .OrderBy(m => m.Gia)
                    .Take(5)
                    .Select(m => new MonAn
                    {
                        MaMonAn = m.MaMonAn,
                        MaNhaHang = m.MaNhaHang,
                        TenMonAn = m.TenMonAn,
                        MoTa = m.MoTa,
                        Gia = m.Gia,
                        UrlhinhAnh = m.UrlhinhAnh,
                        NgayTao = m.NgayTao
                    })
                    .ToListAsync();

                if (cheapestFoods == null || cheapestFoods.Count == 0)
                {
                    return NotFound("Không tìm thấy món ăn");
                }

                return cheapestFoods;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting cheapest foods");
                return StatusCode(500, "Đã xảy ra lỗi khi lấy món ăn rẻ nhất");
            }
        }

        // GET: api/MonAn/search?keyword=XXX
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<MonAn>>> SearchFoods(string keyword)
        {
            try
            {
                if (string.IsNullOrEmpty(keyword))
                    return new List<MonAn>();

                var searchResults = await _context.MonAns
                    .Where(m => m.TenMonAn.Contains(keyword) || 
                                (m.MoTa != null && m.MoTa.Contains(keyword)))
                    .Take(10)
                    .Select(m => new MonAn
                    {
                        MaMonAn = m.MaMonAn,
                        MaNhaHang = m.MaNhaHang,
                        TenMonAn = m.TenMonAn,
                        MoTa = m.MoTa,
                        Gia = m.Gia,
                        UrlhinhAnh = m.UrlhinhAnh,
                        NgayTao = m.NgayTao
                    })
                    .ToListAsync();

                return searchResults;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching foods with keyword {Keyword}", keyword);
                return StatusCode(500, "Đã xảy ra lỗi khi tìm kiếm món ăn");
            }
        }

        // PUT: api/MonAn/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMonAn(int id, MonAn monAn)
        {
            if (id != monAn.MaMonAn)
            {
                return BadRequest("ID không khớp");
            }

            _context.Entry(monAn).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Đã xảy ra lỗi khi cập nhật món ăn");
            }
        }

        // POST: api/MonAn
        [HttpPost]
        public async Task<ActionResult<MonAn>> PostMonAn(MonAn monAn)
        {
            try
            {
                // Tách đối tượng MaNhaHangNavigation để tránh thêm mới
                var maNhaHang = monAn.MaNhaHang;
                monAn.MaNhaHangNavigation = null;

                _context.MonAns.Add(monAn);
                await _context.SaveChangesAsync();

                // Lấy thông tin đầy đủ về món ăn để trả về
                var result = await _context.MonAns
                    .Include(m => m.MaNhaHangNavigation)
                    .FirstOrDefaultAsync(m => m.MaMonAn == monAn.MaMonAn);

                return CreatedAtAction("GetMonAn", new { id = monAn.MaMonAn }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Đã xảy ra lỗi khi tạo món ăn mới");
            }
        }
        // DELETE: api/MonAn/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMonAn(int id)
        {
            try
            {
                var monAn = await _context.MonAns.FindAsync(id);
                if (monAn == null)
                {
                    return NotFound("Không tìm thấy món ăn");
                }

                _context.MonAns.Remove(monAn);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Đã xảy ra lỗi khi xóa món ăn");
            }
        }
    }
}
