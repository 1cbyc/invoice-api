using System.ComponentModel.DataAnnotations;

namespace InvoiceApi.DTOs;

public class UpdateInvoiceDto
{
    [StringLength(50)]
    public string? InvoiceNumber { get; set; }

    public DateTime? IssueDate { get; set; }

    public DateTime? DueDate { get; set; }

    [StringLength(200)]
    public string? CustomerName { get; set; }

    [StringLength(500)]
    public string? CustomerAddress { get; set; }

    [StringLength(100)]
    [EmailAddress]
    public string? CustomerEmail { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? TaxAmount { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? DiscountAmount { get; set; }

    [StringLength(20)]
    public string? Status { get; set; }

    [StringLength(1000)]
    public string? Notes { get; set; }

    public List<CreateInvoiceItemDto>? Items { get; set; }
}

