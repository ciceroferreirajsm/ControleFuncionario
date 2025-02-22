using ControleFuncionario.Application.Repository;
using ControleFuncionario.Application.Repository.Interfaces;
using ControleFuncionario.Application.Services;
using ControleFuncionario.Application.Services.Interfaces;
using ControleFuncionario.Mapper;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = "https://localhost:7140", 
            ValidAudience = "http://localhost:3000",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());
});

builder.Services.AddAuthorization();

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Executa a configuração do banco de dados
var databaseSetup = new DatabaseSetup(connectionString);
databaseSetup.Setup();

builder.Services.AddSingleton<IFuncionarioRepository, FuncionarioRepository>();
builder.Services.AddSingleton<ILoginRepository, LoginRepository>();
builder.Services.AddSingleton<IFuncionarioService, FuncionarioService>();
builder.Services.AddSingleton<ILoginService, LoginService>();

builder.Services.AddAutoMapper(typeof(FuncionarioProfile));


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Funcionários API",
        Version = "v1",
        Description = "API para gerenciar funcionários",
    });

    var xmlFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MovimentacaoAPI.xml");

    Console.WriteLine("Diretório base: " + AppDomain.CurrentDomain.BaseDirectory);
    Console.WriteLine("Caminho do XML: " + xmlFile);

    if (File.Exists(xmlFile))
    {
        c.IncludeXmlComments(xmlFile);
    }
    else
    {
        Console.WriteLine("Arquivo XML não encontrado: " + xmlFile);
    }
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization(); 
app.MapControllers();
app.UseCors("AllowAllOrigins");
app.UseMiddleware<RequestResponseLoggingMiddleware>();
app.Run();
