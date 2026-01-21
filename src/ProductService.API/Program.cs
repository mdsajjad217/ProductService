using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using ProductService.API.Middleware;
using ProductService.Application.Service;
using ProductService.Domain.Option;
using ProductService.Domain.Repository;
using ProductService.Domain.Service;
using ProductService.Domain.Validator;
using ProductService.Infrastructure.Consumer;
using ProductService.Infrastructure.Persistence;
using ProductService.Infrastructure.Repository.EfProductRepository;

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
builder.Services.AddHostedService<KafkaConsumerWorker>();

builder.WebHost.UseUrls("http://0.0.0.0:8080");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}

app.UseMiddleware<ExceptionMiddleware>();

//app.UseHttpsRedirection();

app.UseRouting();
app.MapDefaultControllerRoute();
app.MapHealthChecks("/health");
app.Run();