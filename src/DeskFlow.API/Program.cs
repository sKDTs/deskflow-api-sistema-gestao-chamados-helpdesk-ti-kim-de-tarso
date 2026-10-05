using DeskFlow.API.Data;
using DeskFlow.API.Repositories;
using DeskFlow.API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<DeskFlowDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<CategoriaRepository>();
builder.Services.AddScoped<CategoriaService>();

builder.Services.AddScoped<ChamadoRepository>();
builder.Services.AddScoped<ChamadoService>();

builder.Services.AddScoped<InteracaoRepository>();
builder.Services.AddScoped<InteracaoService>();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapControllers();

app.Run();
