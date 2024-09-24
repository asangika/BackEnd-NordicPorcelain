using Ecommerce.Domain.src.Interfaces;
using Ecommerce.Infrastructure.src.Repository;
using Ecommerce.Infrastructure.src.Database;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Ecommerce.Service.src.AuthService;
using Ecommerce.Infrastructure.src.Repository.Service;
using Ecommerce.Service.src.UserService;
using Newtonsoft.Json.Converters;
using Microsoft.OpenApi.Models;
using Ecommerce.Presentation.src.Middleware;
using Ecommerce.Service.src.CategoryService;
using Ecommerce.Service.src.ProductService;
using Ecommerce.Service.src.ProductImageService;
using Ecommerce.Service.src.OrderService;
using Ecommerce.Service.src.OrderItemService;
using Ecommerce.Service.src.NotificationService;
using Ecommerce.Service.src.OrderService.Handlers;
using Ecommerce.Service.src.AddressService;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.Converters.Add(new StringEnumConverter());
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    options =>
    {
        options.SwaggerDoc("v1", new() { Title = "Ecommerce", Version = "v1" });
        options.SchemaFilter<EnumSchemaFilter>();
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
        {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\""
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
    });

// Configure CORS policy to allow requests from your frontend (http://localhost:5173)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontendApp",
        builder => builder.WithOrigins("http://localhost:5173")
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

// Add database context into app
builder.Services.AddDbContext<ApplicationDbContext>(options =>

    options
    .EnableSensitiveDataLogging()
    .LogTo(Console.WriteLine, LogLevel.Information)
    .AddInterceptors(new TimeStampInterceptor())
    .UseNpgsql(builder.Configuration.GetConnectionString("localhost"))
    .UseSnakeCaseNamingConvention());


// Dependency Injection
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserAddressRepository, UserAddressRepository>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IShipmentRepository, ShipmentRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductImageRepository, ProductImageRepository>();
builder.Services.AddScoped<ExceptionHandlerMiddleware>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthManagement, AuthManagement>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IUserManagement, UserManagement>();
builder.Services.AddScoped<ICategoryManagement, CategoryManagement>();
builder.Services.AddScoped<IProductManagement, ProductManagement>();
builder.Services.AddScoped<IProductImageManagement, ProductImageManagement>();
builder.Services.AddScoped<IEmailManagement, EmailManagement>();
builder.Services.AddScoped<ISmsManagement, SmsManagement>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IOrderStatushandler, OrderStatusHandler>();
builder.Services.AddScoped<IOrderManagement, OrderManagement>();
builder.Services.AddScoped<IOrderItemManagement, OrderItemManagement>();
builder.Services.AddScoped<IAddressManagement, AddressManagement>();
builder.Services.AddScoped<ILogger, Logger<OrderManagement>>();


// Add authentication configuration
builder.Services.AddAuthentication(
    options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    }
)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

// Inject middleware to the application

app.UseHttpsRedirection();
app.UseCors("AllowFrontendApp");
//app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();
