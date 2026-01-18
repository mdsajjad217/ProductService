using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ProductService.Application.Service;
using ProductService.Domain.Repository;
using ProductService.Domain.Validator;
using ProductService.Infrastructure.Persistence;
using ProductService.Infrastructure.Repository.EfProductRepository;
using ProductService.Infrastructure.Repository.InMemoryProductRepository;
using ProductService.Domain.Service;
using ProductService.API.Middleware;
using ProductService.Application.Option;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IProductQueryService, ProductQueryService>();
builder.Services.AddScoped<IProductCommandService, ProductCommandService>();
builder.Services.AddScoped<IProductRepository, EfProductRepository>();

builder.Services.AddDbContext<ProductDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ProductDb")));

builder.Services.Configure<KafkaConsumerOptions>(
    builder.Configuration.GetSection("Kafka")
);

builder.Services.AddScoped<IProductRepository, EfProductRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks().AddDbContextCheck<ProductDbContext>();

builder.Services.AddValidatorsFromAssemblyContaining<CreateProductRequestValidator>();
builder.Services.AddFluentValidationAutoValidation();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseRouting();
app.MapDefaultControllerRoute();
app.MapHealthChecks("/health");
app.Run();