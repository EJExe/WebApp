using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjectManagement.Application.Services;
using ProjectManagement.Domain;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Domain.Interfaces;
using ProjectManagement.Infrastructure;
using ProjectManagement.Infrastructure.Data;
using ProjectManagement.Infrastructure.Repositories;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddDbContext<AppDbContext>(options =>options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    b => b.MigrationsAssembly("ProjectManagement.Infrastructure"))); // явное указание сборки
builder.Services.AddControllers();

// –егистраци€ репозиториев
builder.Services.AddScoped<IBodyTypeRepository, BodyTypeRepository>();
builder.Services.AddScoped<IBrandRepository, BrandRepository>();
builder.Services.AddScoped<ICarCategoryRepository, CarCategoryRepository>();
builder.Services.AddScoped<ICarDriveTypeRepository, CarDriveTypeRepository>();
builder.Services.AddScoped<ICarFeatureRepository, CarFeatureRepository>();
builder.Services.AddScoped<ICarRepository, CarRepository>();
builder.Services.AddScoped<IFuelTypeRepository, FuelTypeRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRentalApplicationRepository, RentalApplicationRepository>();

// –егистраци€ сервисов
builder.Services.AddScoped<BodyTypeService>();
builder.Services.AddScoped<BrandService>();
builder.Services.AddScoped<CarCategoryService>();
builder.Services.AddScoped<CarDriveTypeService>();
builder.Services.AddScoped<CarFeatureService>();
builder.Services.AddScoped<CarService>();
builder.Services.AddScoped<FuelTypeService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<PaymentService>();
builder.Services.AddScoped<ReviewService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<RentalApplicationService>();
builder.Services.AddScoped<ReservationService>();
builder.Services.AddScoped<ReservationRepository>();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddIdentity<User, IdentityRole>(options => {})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "https://localhost:7154")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["SigningKey SigningKey SigningKey SigningKey SigningKey SigningKey SigningKey SigningKey "]))
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("admin"));
    
});



builder.Services.AddEndpointsApiExplorer();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Project Management API V1");
        c.RoutePrefix = "swagger"; 
    });
}


app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
        @"D:\UNIVERS\3 ”–—\3.2\—адыков\ASPNETCore\Lising\images\cars"),
    RequestPath = "/images/cars" // URL-путь дл€ доступа к файлам
});
//app.UseAuthorization();

//app.MapControllers();

app.UseCors("AllowReactApp");

app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    var userManager = services.GetRequiredService<UserManager<User>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    await DbInitializer.Initialize(context, userManager, roleManager);
}

app.Run();
