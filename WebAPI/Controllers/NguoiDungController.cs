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
    public class NguoiDungController : ControllerBase
    {
        private readonly FoodOrderDBContext _context;

        public NguoiDungController(FoodOrderDBContext context)
        {
            _context = context;
        }

        // GET: api/NguoiDung
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NguoiDung>>> GetNguoiDungs()
        {
            return await _context.NguoiDungs.ToListAsync();
        }

        // GET: api/NguoiDung/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NguoiDung>> GetNguoiDung(int id)
        {
            var nguoiDung = await _context.NguoiDungs.FindAsync(id);

            if (nguoiDung == null)
            {
                return NotFound();
            }

            return nguoiDung;
        }

        // PUT: api/NguoiDung/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNguoiDung(int id, NguoiDung nguoiDung)
        {
            if (id != nguoiDung.MaNguoiDung)
            {
                return BadRequest();
            }

            _context.Entry(nguoiDung).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NguoiDungExists(id))
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

        // POST: api/NguoiDung
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<NguoiDung>> PostNguoiDung(NguoiDung nguoiDung)
        {
            _context.NguoiDungs.Add(nguoiDung);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNguoiDung", new { id = nguoiDung.MaNguoiDung }, nguoiDung);
        }

        // DELETE: api/NguoiDung/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNguoiDung(int id)
        {
            var nguoiDung = await _context.NguoiDungs.FindAsync(id);
            if (nguoiDung == null)
            {
                return NotFound();
            }

            _context.NguoiDungs.Remove(nguoiDung);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NguoiDungExists(int id)
        {
            return _context.NguoiDungs.Any(e => e.MaNguoiDung == id);
        }
    }
}
