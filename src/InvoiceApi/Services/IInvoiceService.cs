using InvoiceApi.DTOs;

namespace InvoiceApi.Services;

public interface IInvoiceService
{
    Task<IEnumerable<InvoiceDto>> GetAllInvoicesAsync();
    Task<InvoiceDto?> GetInvoiceByIdAsync(int id);
    Task<InvoiceDto> CreateInvoiceAsync(CreateInvoiceDto createInvoiceDto);
    Task<InvoiceDto?> UpdateInvoiceAsync(int id, UpdateInvoiceDto updateInvoiceDto);
    Task<bool> DeleteInvoiceAsync(int id);
    Task<IEnumerable<InvoiceDto>> GetInvoicesByStatusAsync(string status);
    Task<IEnumerable<InvoiceDto>> GetInvoicesByCustomerAsync(string customerName);
}

