using System.ComponentModel.DataAnnotations;

namespace InvoiceApi.DTOs;

public class CreateInvoiceItemDto
{
    [Required]
    [StringLength(200)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
    public decimal Quantity { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal UnitPrice { get; set; }

    [Range(0, 100)]
    public decimal TaxRate { get; set; }
}

