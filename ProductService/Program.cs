using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Services;
using ProductService.Services.Interfaces;
using ProductService.Services.Implementations;
using ProductService.MappingProfiles;
using System.Reflection;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// Register DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// AutoMapper
// builder.Services.AddAutoMapper(typeof(ProductSizeMappingProfile));
// builder.Services.AddAutoMapper(typeof(Program));
// builder.Services.AddAutoMapper(typeof(Program).Assembly);
// builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
// var mapperConfig = new MapperConfiguration(cfg =>
// {
//     cfg.AddProfile<ProductSizeMappingProfile>();
// });
// var mapper = mapperConfig.CreateMapper();
// builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(typeof(ProductSizeMappingProfile));

// builder.Services.AddAutoMapper(typeof(ProductSizeMappingProfile));
// builder.Services.AddAutoMapper(typeof(ProductSizeMappingProfile));

// Register ProductService
builder.Services.AddScoped<IProductService, ProductManagementService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductSizeService, ProductSizeService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    SeedData.Initialize(context);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();
