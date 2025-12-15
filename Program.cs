using Microsoft.EntityFrameworkCore;
using InvoiceApi.Data;
using InvoiceApi.Services;
using InvoiceApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add Entity Framework with PostgreSQL
// Support DATABASE_URL (common in Fly.io) or ConnectionStrings__DefaultConnection
// DATABASE_URL has highest priority (cloud environments)
var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");
if (!string.IsNullOrEmpty(connectionString))
{
    // Parse DATABASE_URL format: postgres://user:password@host:port/database
    var uri = new Uri(connectionString);
    var username = uri.UserInfo.Split(':')[0];
    var password = uri.UserInfo.Split(':').Length > 1 ? uri.UserInfo.Split(':')[1] : "";
    connectionString = $"Host={uri.Host};Port={uri.Port};Database={uri.AbsolutePath.TrimStart('/')};Username={username};Password={password};SSL Mode=Require;Trust Server Certificate=true";
}
else
{
    // Fall back to configuration (supports ConnectionStrings__DefaultConnection env var or appsettings.json)
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
}

builder.Services.AddDbContext<InvoiceDbContext>(options =>
    options.UseNpgsql(connectionString ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.")));

// Add services
builder.Services.AddScoped<IInvoiceService, InvoiceService>();

// Add Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "Invoice API",
        Version = "v1",
        Description = "A comprehensive REST API for managing invoices"
    });
});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Apply database migrations on startup (if any exist)
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<InvoiceDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    try
    {
        // Check if database exists and can be migrated
        if (dbContext.Database.GetPendingMigrations().Any())
        {
            logger.LogInformation("Applying database migrations...");
            dbContext.Database.Migrate();
            logger.LogInformation("Database migrations applied successfully.");
        }
        else
        {
            // If no migrations exist, ensure database is created
            dbContext.Database.EnsureCreated();
            logger.LogInformation("Database ensured created.");
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while setting up the database.");
        // In production, you might want to fail fast here
        // throw;
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Invoice API v1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

// Add error handling middleware
app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
