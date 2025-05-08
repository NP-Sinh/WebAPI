using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Text;

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

                // Xuất danh sách món ăn vào file CSV
                await ExportMonAnToCSV();

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
                await ExportMonAnToCSV(); // Xuất dữ liệu ra file CSV sau khi cập nhật
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

                await ExportMonAnToCSV(); // Xuất dữ liệu ra file CSV sau khi thêm mới
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

                await ExportMonAnToCSV(); // Xuất dữ liệu ra file CSV sau khi xóa
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Đã xảy ra lỗi khi xóa món ăn");
            }
        }
        // Phương thức xuất dữ liệu món ăn ra file CSV
        private async Task ExportMonAnToCSV()
        {
            try
            {
                // Đường dẫn file CSV
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Data", "datamonan.csv");
                
                // Đảm bảo thư mục tồn tại
                string directoryPath = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // Lấy dữ liệu món ăn từ database
                var monAns = await _context.MonAns
                    .Include(m => m.MaNhaHangNavigation)
                    .ToListAsync();

                // Tạo nội dung file CSV
                StringBuilder csv = new StringBuilder();
                
                // Thêm header
                csv.AppendLine("MaMonAn,MaNhaHang,TenMonAn,MoTa,Gia,UrlHinhAnh,NgayTao,TenNhaHang");
                
                // Thêm dữ liệu
                foreach (var monAn in monAns)
                {
                    string tenMonAn = monAn.TenMonAn?.Replace(",", ";");
                    string moTa = monAn.MoTa?.Replace(",", ";");
                    string tenNhaHang = monAn.MaNhaHangNavigation?.TenNhaHang?.Replace(",", ";");
                    
                    csv.AppendLine($"{monAn.MaMonAn},{monAn.MaNhaHang},\"{tenMonAn}\",\"{moTa}\",{monAn.Gia},\"{monAn.UrlhinhAnh}\",\"{monAn.NgayTao?.ToString("yyyy-MM-dd HH:mm:ss")}\",\"{tenNhaHang}\"");
                }

                // Ghi file
                await System.IO.File.WriteAllTextAsync(filePath, csv.ToString(), Encoding.UTF8);
                _logger.LogInformation($"Đã xuất dữ liệu món ăn ra file CSV: {filePath}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xuất dữ liệu món ăn ra file CSV");
            }
        }

        // GET: api/MonAn/ExportCSV
        [HttpGet("ExportCSV")]
        public async Task<IActionResult> ExportCSVManual()
        {
            try
            {
                await ExportMonAnToCSV();
                return Ok("Đã xuất dữ liệu món ăn ra file CSV thành công");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Đã xảy ra lỗi khi xuất dữ liệu món ăn ra file CSV");
            }
        }

        // GET: api/MonAn/GetCSVData
        [HttpGet("GetCSVData")]
        public async Task<IActionResult> GetCSVData()
        {
            try
            {
                // Đảm bảo file CSV đã được cập nhật với dữ liệu mới nhất
                await ExportMonAnToCSV();
                
                // Đường dẫn file CSV
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Data", "datamonan.csv");
                
                // Kiểm tra file tồn tại
                if (!System.IO.File.Exists(filePath))
                {
                    return NotFound("File CSV không tồn tại");
                }
                
                // Đọc nội dung file
                string csvContent = await System.IO.File.ReadAllTextAsync(filePath, Encoding.UTF8);
                
                // Trả về nội dung file dưới dạng text/csv
                return Content(csvContent, "text/csv", Encoding.UTF8);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi đọc dữ liệu từ file CSV");
                return StatusCode(500, "Đã xảy ra lỗi khi đọc dữ liệu từ file CSV");
            }
        }
    }
}
