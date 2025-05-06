using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChiTietDonHangController : ControllerBase
    {
        private readonly FoodOrderDBContext _context;

        public ChiTietDonHangController(FoodOrderDBContext context)
        {
            _context = context;
        }

        // GET: api/ChiTietDonHang
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChiTietDonHang>>> GetChiTietDonHangs()
        {
            return await _context.ChiTietDonHangs.ToListAsync();
        }

        // GET: api/ChiTietDonHang/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ChiTietDonHang>> GetChiTietDonHang(int id)
        {
            var chiTietDonHang = await _context.ChiTietDonHangs.FindAsync(id);

            if (chiTietDonHang == null)
            {
                return NotFound();
            }

            return chiTietDonHang;
        }

        // PUT: api/ChiTietDonHang/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChiTietDonHang(int id, ChiTietDonHang chiTietDonHang)
        {
            if (id != chiTietDonHang.MaChiTietDonHang)
            {
                return BadRequest();
            }

            _context.Entry(chiTietDonHang).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChiTietDonHangExists(id))
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

        // POST: api/ChiTietDonHang
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ChiTietDonHang>> PostChiTietDonHang(ChiTietDonHang chiTietDonHang)
        {
            _context.ChiTietDonHangs.Add(chiTietDonHang);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetChiTietDonHang", new { id = chiTietDonHang.MaChiTietDonHang }, chiTietDonHang);
        }

        // DELETE: api/ChiTietDonHang/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChiTietDonHang(int id)
        {
            var chiTietDonHang = await _context.ChiTietDonHangs.FindAsync(id);
            if (chiTietDonHang == null)
            {
                return NotFound();
            }

            _context.ChiTietDonHangs.Remove(chiTietDonHang);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ChiTietDonHangExists(int id)
        {
            return _context.ChiTietDonHangs.Any(e => e.MaChiTietDonHang == id);
        }
    }
}
