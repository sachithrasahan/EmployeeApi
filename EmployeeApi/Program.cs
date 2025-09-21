using EmployeeApi.Data;
using EmployeeApi.Mapping;
using EmployeeApi.Middleware;
using EmployeeApi.Models;
using EmployeeApi.Services;
using EmployeeApi.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Controllers
builder.Services.AddControllers();

// DI
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IValidator<EmployeeRequestDto>, EmployeeRequestDtoValidator>();
builder.Services.AddScoped<IValidator<EmployeeUpdateRequestDto>, EmployeeUpdateRequestDtoValidator>();

// Swagger (optional)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware
app.UseMiddleware<ErrorHandlingMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();
