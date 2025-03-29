using FamilyBudgetBackend.Data;
using FamilyBudgetBackend.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);


// === 1. ����������� �������� ===

// ��������� ������� ������������
builder.Services.AddControllers();

// ��������� Swagger
builder.Services.AddEndpointsApiExplorer(); // ��������� ��� Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Family Budget API",
        Version = "v1",
        Description = "API ��� ���������� �������� ��������",
        Contact = new OpenApiContact { Name = "Nikita M.", Email = "dev@example.com" }
    });
}); // ���������� UI ��� Swagger

// ��������� �������� �� (SQLite)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// ��������� CORS (��������� ������� � ������ ������)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()  // ��������� ����� �����
              .AllowAnyMethod()  // ����� HTTP-����� (GET, POST...)
              .AllowAnyHeader(); // ����� ���������
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var db = services.GetRequiredService<ApplicationDbContext>();
        db.Database.Migrate(); // ��������� ��������

        // Seed initial data
        if (!db.TransactionTypes.Any())
        {
            db.TransactionTypes.AddRange(
                new TransactionType { Id = 1, Name = "�����" },
                new TransactionType { Id = 2, Name = "������" }
            );

            db.Categories.AddRange(
                new Category { Id = 1, Name = "��������", TransactionTypeId = 1 },
                new Category { Id = 2, Name = "��������", TransactionTypeId = 2 }
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

// �������� Swagger ������ � Development-������
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(c =>
    {
        c.SerializeAsV2 = true;
    }); // �������� ��������� JSON-������������
    app.UseSwaggerUI(); // �������� Swagger UI
}

// === 2. ������������ middleware ===

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseCors("AllowAll");  // �������� CORS
app.MapControllers();     // ���������� �����������

// === 3. ������ ������� ===
app.Run();