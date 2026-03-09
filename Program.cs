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
builder.Services.AddScoped<IPedidoService, PedidoService>();
builder.Services.AddScoped<IProdutoService, ProdutoService>();
builder.Services.AddScoped<IClienteService, ClienteService>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo 
    { 
        Title = "Pizzaria API", 
        Version = "v1",
        Description = "Sistema de gestão de pedidos para pizzarias."
    });

    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});


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