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

invoice-api/
├── Controllers/          InvoicesController.cs
├── Data/                 InvoiceDbContext.cs
├── DTOs/                 All 5 DTOs present
├── Middleware/           ErrorHandlingMiddleware.cs
├── Models/               Invoice.cs, InvoiceItem.cs
├── Properties/           launchSettings.json
├── Services/             IInvoiceService.cs, InvoiceService.cs
├── InvoiceApi.csproj     PostgreSQL configured
├── Program.cs            Full configuration
├── appsettings.json      PostgreSQL connection string
├── Dockerfile            For containerization
├── fly.toml              For Fly.io deployment
└── 
