using FamilyBudgetBackend.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ���������� ��������
builder.Services.AddControllers(); // ��������� ������������
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// ��������� ���� ������
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
           .LogTo(Console.WriteLine, LogLevel.Information);
});


var app = builder.Build();

// ������������ HTTP-���������
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// ������������� ������������
app.MapControllers();

app.Run();