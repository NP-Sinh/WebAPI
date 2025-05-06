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
    public class ChatHistoryController : ControllerBase
    {
        private readonly FoodOrderDBContext _context;
        private readonly ILogger<ChatHistoryController> _logger;

        public ChatHistoryController(FoodOrderDBContext context, ILogger<ChatHistoryController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/ChatHistory
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChatHistory>>> GetChatHistories()
        {
            try
            {
                return await _context.ChatHistories
                    .OrderByDescending(h => h.Timestamp)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting chat histories");
                return StatusCode(500, "Đã xảy ra lỗi khi lấy lịch sử chat");
            }
        }

        // GET: api/ChatHistory/user/5
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<ChatHistory>>> GetChatHistoryByUser(int userId)
        {
            try
            {
                var chatHistories = await _context.ChatHistories
                    .Where(h => h.UserId == userId)
                    .OrderByDescending(h => h.Timestamp)
                    .ToListAsync();

                if (chatHistories == null || chatHistories.Count == 0)
                {
                    return NotFound("Không tìm thấy lịch sử chat cho người dùng này");
                }

                return chatHistories;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting chat histories for user {UserId}", userId);
                return StatusCode(500, "Đã xảy ra lỗi khi lấy lịch sử chat của người dùng");
            }
        }

        // GET: api/ChatHistory/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ChatHistory>> GetChatHistory(int id)
        {
            try
            {
                var chatHistory = await _context.ChatHistories.FindAsync(id);

                if (chatHistory == null)
                {
                    return NotFound("Không tìm thấy lịch sử chat");
                }

                return chatHistory;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting chat history {Id}", id);
                return StatusCode(500, "Đã xảy ra lỗi khi lấy lịch sử chat");
            }
        }

        // POST: api/ChatHistory
        [HttpPost]
        public async Task<ActionResult<ChatHistory>> PostChatHistory(ChatHistory chatHistory)
        {
            try
            {
                // Đảm bảo timestamp được set
                if (chatHistory.Timestamp == default)
                {
                    chatHistory.Timestamp = DateTime.Now;
                }

                _context.ChatHistories.Add(chatHistory);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetChatHistory", new { id = chatHistory.Id }, chatHistory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating chat history");
                return StatusCode(500, "Đã xảy ra lỗi khi tạo lịch sử chat");
            }
        }

        // DELETE: api/ChatHistory/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChatHistory(int id)
        {
            try
            {
                var chatHistory = await _context.ChatHistories.FindAsync(id);
                if (chatHistory == null)
                {
                    return NotFound("Không tìm thấy lịch sử chat");
                }

                _context.ChatHistories.Remove(chatHistory);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting chat history {Id}", id);
                return StatusCode(500, "Đã xảy ra lỗi khi xóa lịch sử chat");
            }
        }

        // DELETE: api/ChatHistory/user/5
        [HttpDelete("user/{userId}")]
        public async Task<IActionResult> DeleteChatHistoryByUser(int userId)
        {
            try
            {
                var chatHistories = await _context.ChatHistories
                    .Where(h => h.UserId == userId)
                    .ToListAsync();

                if (chatHistories == null || chatHistories.Count == 0)
                {
                    return NotFound("Không tìm thấy lịch sử chat cho người dùng này");
                }

                _context.ChatHistories.RemoveRange(chatHistories);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting chat histories for user {UserId}", userId);
                return StatusCode(500, "Đã xảy ra lỗi khi xóa lịch sử chat của người dùng");
            }
        }
    }
} 