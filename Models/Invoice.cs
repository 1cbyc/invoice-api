using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceApi.Models;

public class Invoice
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string InvoiceNumber { get; set; } = string.Empty;

    [Required]
    public DateTime IssueDate { get; set; }

    [Required]
    public DateTime DueDate { get; set; }

    [Required]
    [StringLength(200)]
    public string CustomerName { get; set; } = string.Empty;

    [StringLength(500)]
    public string? CustomerAddress { get; set; }

    [StringLength(100)]
    public string? CustomerEmail { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal SubTotal { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TaxAmount { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal DiscountAmount { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }

    [StringLength(20)]
    public string Status { get; set; } = "Draft"; // Draft, Sent, Paid, Overdue, Cancelled

    [StringLength(1000)]
    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    // Navigation property
    public virtual ICollection<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();
}

