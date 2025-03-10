using Infrastructure.Data.Contexts;
using Application.Interfaces.Services;
using Application.Interfaces.Repository;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Services;

// Initialize builder
var builder = WebApplication.CreateBuilder(args);

// Config Service
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<PayaDbContext>(option=>option.UseSqlServer("Data Source=DESKTOP-9PPD55B; Initial Catalog=Bank; Integrated Security=true; TrustServerCertificate=True"));
builder.Services.AddScoped<ITransactionRepository,TransactionRepository>();
builder.Services.AddScoped<ITransactionService,TransactionService>();

// Initialize app
var app = builder.Build();

// Config tools
app.MapGet("/", () => "Hello World!");
app.MapControllers();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
