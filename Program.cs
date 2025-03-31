using FamilyBudgetBackend.Data;
using FamilyBudgetBackend.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);


// Registration of services

// Adding controller services
builder.Services.AddControllers();

// Add Swagger
builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Family Budget API",
        Version = "v1",
        Description = "API для управления семейным бюджетом",
        Contact = new OpenApiContact { Name = "Nikita M.", Email = "dev@example.com" }
    });
}); //Generates UI for Swagger. Unnecessary since the problem was elsewhere.

// Adding a DB context (SQLite)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add CORS (allow requests from any domain)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()  // We allow any domain
              .AllowAnyMethod()  // Any HTTP method (GET, POST...)
              .AllowAnyHeader(); // Any headings
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var db = services.GetRequiredService<ApplicationDbContext>();
        db.Database.Migrate();

        // Seed initial data
        if (!db.TransactionTypes.Any())
        {
            db.TransactionTypes.AddRange(
                new TransactionType { Id = 1, Name = "Доход" },
                new TransactionType { Id = 2, Name = "Расход" }
            );

            db.Categories.AddRange(
                new Category { Id = 1, Name = "Зарплата", TransactionTypeId = 1 },
                new Category { Id = 2, Name = "Продукты", TransactionTypeId = 2 }
            );

            await db.SaveChangesAsync();
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating or seeding the database.");
    }
}

// Enable Swagger only in Development mode
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(c =>
    {
        c.SerializeAsV2 = true;
    }); 
    app.UseSwaggerUI(); // Enables Swagger UI
}

// Middleware Configuration

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseCors("AllowAll");  // Enable CORS
app.MapControllers();     // Connecting controllers

// Starting the server
app.Run();