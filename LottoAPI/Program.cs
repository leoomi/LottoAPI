using LottoAPI.Application.Features.NumberChecking;
using LottoAPI.Infrastructure;
using LottoAPI.Infrastructure.Repositories;
using LottoAPI.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddHttpClient();
builder.Services.AddDbContext<LottoDbContext>(options => options.UseSqlite("Data Source=app.db"));

builder.Services.AddScoped<ILottoRepository, LottoRepository>();
builder.Services.AddScoped<IResultRepository, ResultRepository>();
builder.Services.AddScoped<IExternalLottoService, CaixaLottoService>();
builder.Services.AddScoped<ICheckLottoLineUseCase, CheckLottoLineHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUi(options =>
    {
        options.DocumentPath = "/openapi/v1.json";
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
