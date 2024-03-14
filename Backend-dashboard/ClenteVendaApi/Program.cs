using Dominio.Data;
using Dominio.Models.DTO;
using Dominio.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Generics;
using Service.GenericService;
using Service.ClienteServices;
using Repository.ClienteRepositorys;
using Repository.VendasRepositorys;
using Service.VendaService;

var builder = WebApplication.CreateBuilder(args);

// Configuração do banco de dados
builder.Services.AddDbContext<AplicacaoDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ClientesVendas"));
});

// Registro dos serviços necessários
builder.Services.AddControllers();

builder.Services.AddScoped<IGenericRepository<Cliente>, GenericRepository<Cliente>>();
builder.Services.AddScoped<IGenericRepository<Venda>, GenericRepository<Venda>>();
builder.Services.AddScoped<IGenericService<Venda, VendaDTO, Venda>, GenericService<Venda, VendaDTO, Venda>>();
builder.Services.AddScoped<IGenericService<Cliente, ClienteDTO, Cliente>, GenericService<Cliente, ClienteDTO, Cliente>>();
builder.Services.AddScoped<IVendaRepository, VendaRepository>();
builder.Services.AddScoped<IVendaService, VendaService>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IClienteService, ClienteService>();

builder.Services.AddAutoMapper(typeof(Venda));
builder.Services.AddAutoMapper(typeof(Cliente));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:8080")
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

var app = builder.Build();
app.UseCors();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AplicacaoDbContext>();
    db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error-development");
    app.UseSwagger();
    app.UseSwaggerUI();
} else
{
    app.UseExceptionHandler("/error");
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
