using FamilyBudgetBackend.Data;
using FamilyBudgetBackend.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);


// === 1. Регистрация сервисов ===

// Добавляем сервисы контроллеров
builder.Services.AddControllers();

// Добавляем Swagger
builder.Services.AddEndpointsApiExplorer(); // Необходим для Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Family Budget API",
        Version = "v1",
        Description = "API для управления семейным бюджетом",
        Contact = new OpenApiContact { Name = "Nikita M.", Email = "dev@example.com" }
    });
}); // Генерирует UI для Swagger

// Добавляем контекст БД (SQLite)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Добавляем CORS (разрешаем запросы с любого домена)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()  // Разрешаем любой домен
              .AllowAnyMethod()  // Любой HTTP-метод (GET, POST...)
              .AllowAnyHeader(); // Любые заголовки
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var db = services.GetRequiredService<ApplicationDbContext>();
        db.Database.Migrate(); // Применяем миграции

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

// Включаем Swagger только в Development-режиме
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(c =>
    {
        c.SerializeAsV2 = true;
    }); // Включает генерацию JSON-спецификации
    app.UseSwaggerUI(); // Включает Swagger UI
}

// === 2. Конфигурация middleware ===

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseCors("AllowAll");  // Включаем CORS
app.MapControllers();     // Подключаем контроллеры

// === 3. Запуск сервера ===
app.Run();