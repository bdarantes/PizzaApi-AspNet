using Microsoft.EntityFrameworkCore;
using PizzariaApi.Data;
using PizzariaApi.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Extrair a connection string para facilitar o uso
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        connectionString,
        new MySqlServerVersion(new Version(11, 8, 3))
    )
);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IPedidoService, PedidoService>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Comente a linha abaixo se estiver tendo problemas com certificados SSL locais no Linux
// app.UseHttpsRedirection(); 

app.UseAuthorization();
app.MapControllers();

app.Run();