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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Dominio.Models.ApplicationUser;
using Microsoft.OpenApi.Models;
using Repository.TokenRepository;
using Service.AuthService.RolesServices;
using Service.AuthService.AuthService;
using Service.AuthService.Cadastro;

var builder = WebApplication.CreateBuilder(args);

// Configuração do banco de dados
builder.Services.AddDbContext<AplicacaoDbContext>( options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ClientesVendas"));
});

builder.Services.AddControllers();

// Registro dos serviços necessários
builder.Services.AddControllers();
builder.Services.AddScoped<IGenericRepository<Cliente>, GenericRepository<Cliente>>();
builder.Services.AddScoped<IGenericRepository<Venda>, GenericRepository<Venda>>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IVendaRepository, VendaRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IGenericService<Venda, VendaDTO, Venda>, GenericService<Venda, VendaDTO, Venda>>();
builder.Services.AddScoped<IGenericService<Cliente, ClienteDTO, Cliente>, GenericService<Cliente, ClienteDTO, Cliente>>();
builder.Services.AddScoped<IVendaService, VendaService>();
builder.Services.AddScoped<IGenericService, ClienteService>();
builder.Services.AddScoped <IAplicationUserService, AplicationUserService>();
builder.Services.AddScoped<IRolesService, RolesService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

builder.Services.AddAutoMapper(typeof(Venda));
builder.Services.AddAutoMapper(typeof(Cliente));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "apiVendasCliente", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Bearer JWT"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
     {
         {
             new OpenApiSecurityScheme
             {
                 Reference = new OpenApiReference
                 {
                     Type = ReferenceType.SecurityScheme,
                     Id = "Bearer"
                 }
             },
             new string[] {}
         }
     });
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>() 
                .AddEntityFrameworkStores<AplicacaoDbContext>()
                .AddDefaultTokenProviders();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:8080", "https://localhost:44345")
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

//Obter a chave secreta
var secretKey = builder.Configuration["JWT:SecretKey"] ?? throw new ArgumentException("Chave secreta invalida");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer( options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});
 builder.Services.AddAuthorization();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("SuperAdminOnly", policy => policy.RequireRole("Admin").RequireClaim("id", "keila"));
    options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
    options.AddPolicy("ExclusivePolicyOnly", policy => 
                     policy.RequireAssertion(context =>
                     context.User.HasClaim(claim => 
                                           claim.Type == "id" && claim.Value == "keila") 
                                           || context.User.IsInRole("SuperAdmin")));
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AplicacaoDbContext>();
    db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseCors();
app.Run();
