using System.ComponentModel.DataAnnotations;

namespace InvoiceApi.DTOs;

public class CreateInvoiceDto
{
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
    [EmailAddress]
    public string? CustomerEmail { get; set; }

    [Range(0, double.MaxValue)]
    public decimal TaxAmount { get; set; }

    [Range(0, double.MaxValue)]
    public decimal DiscountAmount { get; set; }

    [StringLength(20)]
    public string Status { get; set; } = "Draft";

    [StringLength(1000)]
    public string? Notes { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "At least one item is required")]
    public List<CreateInvoiceItemDto> Items { get; set; } = new();
}

