# API Endpoints

| HTTP Verb | Route                                         | Description                                 |
|-----------|-----------------------------------------------|---------------------------------------------|
| GET       | /api/invoices                                 | Get all invoices                            |
| GET       | /api/invoices/{id}                            | Get invoice by ID                           |
| POST      | /api/invoices                                 | Create a new invoice                        |
| PUT       | /api/invoices/{id}                            | Update an invoice                           |
| DELETE    | /api/invoices/{id}                            | Delete an invoice                           |
| GET       | /api/invoices/status/{status}                 | Get invoices filtered by status             |
| GET       | /api/invoices/customer/{customerName}         | Get invoices filtered by customer           |


**Project Structure Overview**

- **Models:**  
  `Invoice` and `InvoiceItem` entity classes define the data model, establishing proper relationships.

- **DTOs:**  
  Separate Data Transfer Objects for:
    - Creating invoices: `CreateInvoiceDto`  
    - Updating invoices: `UpdateInvoiceDto`  
    - Returning invoices: `InvoiceDto`  
    - Invoice items: `CreateInvoiceItemDto`, `InvoiceItemDto`

- **Data Layer:**  
  `InvoiceDbContext` utilizes Entity Framework Core and PostgreSQL for database access.

- **Services:**  
  `IInvoiceService` interface and its implementation `InvoiceService` encapsulate business logic for invoice management.

- **Controllers:**  
  `InvoicesController` exposes REST API endpoints for invoice operations.

- **Middleware:**  
  Custom error handling middleware for centralized exception management.

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
├── Controllers/           InvoicesController.cs
├── Data/                  InvoiceDbContext.cs
├── DTOs/                  CreateInvoiceDto.cs, UpdateInvoiceDto.cs, InvoiceDto.cs, CreateInvoiceItemDto.cs, InvoiceItemDto.cs
├── Middleware/            ErrorHandlingMiddleware.cs
├── Models/                Invoice.cs, InvoiceItem.cs
├── Properties/            launchSettings.json
├── Services/              IInvoiceService.cs, InvoiceService.cs
├── InvoiceApi.csproj      (Configured for PostgreSQL)
├── Program.cs             (Includes full service, EF Core, Swagger, CORS, Fly.io, error middleware)
├── appsettings.json       (PostgreSQL connection string)
├── Dockerfile             (Containerization)
├── fly.toml               (Fly.io deployment configuration)
└── 

Access Points

- **Swagger UI (API documentation):**
    - [http://localhost:5123](http://localhost:5123)
    - [https://localhost:7271](https://localhost:7271) (if using HTTPS)

- **API endpoints:**
    - Base URL: `http://localhost:5123/api/invoices`
    - Example requests:
        - `GET /api/invoices` &ndash; Get all invoices
        - `POST /api/invoices` &ndash; Create invoice
        - `GET /api/invoices/1` &ndash; Get invoice by ID