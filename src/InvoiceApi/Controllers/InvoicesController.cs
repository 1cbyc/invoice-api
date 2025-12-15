using Microsoft.AspNetCore.Mvc;
using InvoiceApi.DTOs;
using InvoiceApi.Services;

namespace InvoiceApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class InvoicesController : ControllerBase
{
    private readonly IInvoiceService _invoiceService;
    private readonly ILogger<InvoicesController> _logger;

    public InvoicesController(IInvoiceService invoiceService, ILogger<InvoicesController> logger)
    {
        _invoiceService = invoiceService;
        _logger = logger;
    }

    /// <summary>
    /// Get all invoices
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<InvoiceDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<InvoiceDto>>> GetInvoices()
    {
        var invoices = await _invoiceService.GetAllInvoicesAsync();
        return Ok(invoices);
    }

    /// <summary>
    /// Get invoice by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(InvoiceDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<InvoiceDto>> GetInvoice(int id)
    {
        var invoice = await _invoiceService.GetInvoiceByIdAsync(id);
        if (invoice == null)
        {
            return NotFound(new { message = $"Invoice with ID {id} not found." });
        }
        return Ok(invoice);
    }

    /// <summary>
    /// Create a new invoice
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(InvoiceDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<InvoiceDto>> CreateInvoice([FromBody] CreateInvoiceDto createInvoiceDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var invoice = await _invoiceService.CreateInvoiceAsync(createInvoiceDto);
            return CreatedAtAction(nameof(GetInvoice), new { id = invoice.Id }, invoice);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating invoice");
            return StatusCode(500, new { message = "An error occurred while creating the invoice." });
        }
    }

    /// <summary>
    /// Update an existing invoice
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(InvoiceDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<InvoiceDto>> UpdateInvoice(int id, [FromBody] UpdateInvoiceDto updateInvoiceDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var invoice = await _invoiceService.UpdateInvoiceAsync(id, updateInvoiceDto);
        if (invoice == null)
        {
            return NotFound(new { message = $"Invoice with ID {id} not found." });
        }

        return Ok(invoice);
    }

    /// <summary>
    /// Delete an invoice
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteInvoice(int id)
    {
        var deleted = await _invoiceService.DeleteInvoiceAsync(id);
        if (!deleted)
        {
            return NotFound(new { message = $"Invoice with ID {id} not found." });
        }

        return NoContent();
    }

    /// <summary>
    /// Get invoices by status
    /// </summary>
    [HttpGet("status/{status}")]
    [ProducesResponseType(typeof(IEnumerable<InvoiceDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<InvoiceDto>>> GetInvoicesByStatus(string status)
    {
        var invoices = await _invoiceService.GetInvoicesByStatusAsync(status);
        return Ok(invoices);
    }

    /// <summary>
    /// Get invoices by customer name
    /// </summary>
    [HttpGet("customer/{customerName}")]
    [ProducesResponseType(typeof(IEnumerable<InvoiceDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<InvoiceDto>>> GetInvoicesByCustomer(string customerName)
    {
        var invoices = await _invoiceService.GetInvoicesByCustomerAsync(customerName);
        return Ok(invoices);
    }
}

