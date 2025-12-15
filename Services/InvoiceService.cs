using Microsoft.EntityFrameworkCore;
using InvoiceApi.Data;
using InvoiceApi.DTOs;
using InvoiceApi.Models;

namespace InvoiceApi.Services;

public class InvoiceService : IInvoiceService
{
    private readonly InvoiceDbContext _context;

    public InvoiceService(InvoiceDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<InvoiceDto>> GetAllInvoicesAsync()
    {
        var invoices = await _context.Invoices
            .Include(i => i.Items)
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync();

        return invoices.Select(MapToDto);
    }

    public async Task<InvoiceDto?> GetInvoiceByIdAsync(int id)
    {
        var invoice = await _context.Invoices
            .Include(i => i.Items)
            .FirstOrDefaultAsync(i => i.Id == id);

        return invoice == null ? null : MapToDto(invoice);
    }

    public async Task<InvoiceDto> CreateInvoiceAsync(CreateInvoiceDto createInvoiceDto)
    {
        var invoice = new Invoice
        {
            InvoiceNumber = createInvoiceDto.InvoiceNumber,
            IssueDate = createInvoiceDto.IssueDate,
            DueDate = createInvoiceDto.DueDate,
            CustomerName = createInvoiceDto.CustomerName,
            CustomerAddress = createInvoiceDto.CustomerAddress,
            CustomerEmail = createInvoiceDto.CustomerEmail,
            TaxAmount = createInvoiceDto.TaxAmount,
            DiscountAmount = createInvoiceDto.DiscountAmount,
            Status = createInvoiceDto.Status,
            Notes = createInvoiceDto.Notes,
            CreatedAt = DateTime.UtcNow
        };

        // Calculate subtotal from items
        decimal subTotal = 0;
        foreach (var itemDto in createInvoiceDto.Items)
        {
            var lineTotal = itemDto.Quantity * itemDto.UnitPrice * (1 + itemDto.TaxRate / 100);
            subTotal += lineTotal;

            invoice.Items.Add(new InvoiceItem
            {
                Description = itemDto.Description,
                Quantity = itemDto.Quantity,
                UnitPrice = itemDto.UnitPrice,
                TaxRate = itemDto.TaxRate,
                LineTotal = lineTotal
            });
        }

        invoice.SubTotal = subTotal;
        invoice.TotalAmount = subTotal + invoice.TaxAmount - invoice.DiscountAmount;

        _context.Invoices.Add(invoice);
        await _context.SaveChangesAsync();

        return MapToDto(invoice);
    }

    public async Task<InvoiceDto?> UpdateInvoiceAsync(int id, UpdateInvoiceDto updateInvoiceDto)
    {
        var invoice = await _context.Invoices
            .Include(i => i.Items)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (invoice == null)
            return null;

        // Update invoice properties
        if (!string.IsNullOrWhiteSpace(updateInvoiceDto.InvoiceNumber))
            invoice.InvoiceNumber = updateInvoiceDto.InvoiceNumber;

        if (updateInvoiceDto.IssueDate.HasValue)
            invoice.IssueDate = updateInvoiceDto.IssueDate.Value;

        if (updateInvoiceDto.DueDate.HasValue)
            invoice.DueDate = updateInvoiceDto.DueDate.Value;

        if (!string.IsNullOrWhiteSpace(updateInvoiceDto.CustomerName))
            invoice.CustomerName = updateInvoiceDto.CustomerName;

        if (updateInvoiceDto.CustomerAddress != null)
            invoice.CustomerAddress = updateInvoiceDto.CustomerAddress;

        if (updateInvoiceDto.CustomerEmail != null)
            invoice.CustomerEmail = updateInvoiceDto.CustomerEmail;

        if (updateInvoiceDto.TaxAmount.HasValue)
            invoice.TaxAmount = updateInvoiceDto.TaxAmount.Value;

        if (updateInvoiceDto.DiscountAmount.HasValue)
            invoice.DiscountAmount = updateInvoiceDto.DiscountAmount.Value;

        if (!string.IsNullOrWhiteSpace(updateInvoiceDto.Status))
            invoice.Status = updateInvoiceDto.Status;

        if (updateInvoiceDto.Notes != null)
            invoice.Notes = updateInvoiceDto.Notes;

        // Update items if provided
        if (updateInvoiceDto.Items != null && updateInvoiceDto.Items.Any())
        {
            // Remove existing items
            _context.InvoiceItems.RemoveRange(invoice.Items);

            // Add new items
            decimal subTotal = 0;
            foreach (var itemDto in updateInvoiceDto.Items)
            {
                var lineTotal = itemDto.Quantity * itemDto.UnitPrice * (1 + itemDto.TaxRate / 100);
                subTotal += lineTotal;

                invoice.Items.Add(new InvoiceItem
                {
                    Description = itemDto.Description,
                    Quantity = itemDto.Quantity,
                    UnitPrice = itemDto.UnitPrice,
                    TaxRate = itemDto.TaxRate,
                    LineTotal = lineTotal
                });
            }

            invoice.SubTotal = subTotal;
        }

        invoice.TotalAmount = invoice.SubTotal + invoice.TaxAmount - invoice.DiscountAmount;
        invoice.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return MapToDto(invoice);
    }

    public async Task<bool> DeleteInvoiceAsync(int id)
    {
        var invoice = await _context.Invoices.FindAsync(id);
        if (invoice == null)
            return false;

        _context.Invoices.Remove(invoice);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<InvoiceDto>> GetInvoicesByStatusAsync(string status)
    {
        var invoices = await _context.Invoices
            .Include(i => i.Items)
            .Where(i => i.Status == status)
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync();

        return invoices.Select(MapToDto);
    }

    public async Task<IEnumerable<InvoiceDto>> GetInvoicesByCustomerAsync(string customerName)
    {
        var invoices = await _context.Invoices
            .Include(i => i.Items)
            .Where(i => i.CustomerName.Contains(customerName))
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync();

        return invoices.Select(MapToDto);
    }

    private static InvoiceDto MapToDto(Invoice invoice)
    {
        return new InvoiceDto
        {
            Id = invoice.Id,
            InvoiceNumber = invoice.InvoiceNumber,
            IssueDate = invoice.IssueDate,
            DueDate = invoice.DueDate,
            CustomerName = invoice.CustomerName,
            CustomerAddress = invoice.CustomerAddress,
            CustomerEmail = invoice.CustomerEmail,
            SubTotal = invoice.SubTotal,
            TaxAmount = invoice.TaxAmount,
            DiscountAmount = invoice.DiscountAmount,
            TotalAmount = invoice.TotalAmount,
            Status = invoice.Status,
            Notes = invoice.Notes,
            CreatedAt = invoice.CreatedAt,
            UpdatedAt = invoice.UpdatedAt,
            Items = invoice.Items.Select(item => new InvoiceItemDto
            {
                Id = item.Id,
                InvoiceId = item.InvoiceId,
                Description = item.Description,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                TaxRate = item.TaxRate,
                LineTotal = item.LineTotal
            }).ToList()
        };
    }
}

