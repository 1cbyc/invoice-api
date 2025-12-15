API endpoints
GET /api/invoices — Get all invoices
GET /api/invoices/{id} — Get invoice by ID
POST /api/invoices — Create new invoice
PUT /api/invoices/{id} — Update invoice
DELETE /api/invoices/{id} — Delete invoice
GET /api/invoices/status/{status} — Get invoices by status
GET /api/invoices/customer/{customerName} — Get invoices by customer

Project structure
Models: Invoice and InvoiceItem entities with relationships
DTOs: CreateInvoiceDto, UpdateInvoiceDto, InvoiceDto, and related item DTOs
Data: InvoiceDbContext with Entity Framework Core and SQLite
Services: IInvoiceService and InvoiceService with business logic
Controllers: InvoicesController with REST endpoints
Middleware: Error handling middleware

invoice-api/
├── Controllers/
├── Data/
├── DTOs/
├── Middleware/
├── Models/
├── Properties/
├── Services/
├── InvoiceApi.csproj
├── InvoiceApi.slnx
├

