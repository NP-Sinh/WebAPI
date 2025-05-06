using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models;

public partial class ChatHistory
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public int UserId { get; set; }
    
    [Required]
    public string UserMessage { get; set; } = null!;
    
    [Required]
    public string BotResponse { get; set; } = null!;
    
    [Required]
    public DateTime Timestamp { get; set; }
    
    // Navigation property
    [ForeignKey("UserId")]
    public virtual NguoiDung? NguoiDung { get; set; }
} 