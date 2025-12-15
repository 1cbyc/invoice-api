using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceApi.Models;

public class InvoiceItem
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int InvoiceId { get; set; }

    [Required]
    [StringLength(200)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Quantity { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TaxRate { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal LineTotal { get; set; }

    // Navigation property
    [ForeignKey(nameof(InvoiceId))]
    public virtual Invoice Invoice { get; set; } = null!;
}

