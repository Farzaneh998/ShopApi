
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using ShopApi.Application.Behaviors;
using ShopApi.Application.Common.Interfaces;
using ShopApi.Application.Services;
using ShopApi.Infrastructure.Data;
using ShopApi.Infrastructure.Services;
using ShopApi.Infrastructure.Services.Caching;
using ShopApi.Infrastructure.Services.Security;
using ShopApi.Middlewares;
using System.Text;




//serilog config
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Seq("http://localhost:5341")
    .CreateLogger();


var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();//useserilog insted  asp Ilogger

#region(jwt Authentication)
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
        JwtBearerDefaults.AuthenticationScheme;

    options.DefaultChallengeScheme =
        JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters =
        new TokenValidationParameters
        {
            ValidateIssuer = true,

            ValidateAudience = true,

            ValidateLifetime = true,

            ValidateIssuerSigningKey = true,

            ValidIssuer =
                builder.Configuration["Jwt:Issuer"],

            ValidAudience =
                builder.Configuration["Jwt:Audience"],

            IssuerSigningKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        builder.Configuration["Jwt:Key"]!))
        };
});
#endregion

builder.Services.AddControllers();
builder.Services.AddDbContext<ShopDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"));
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();//fluentvalidator

//builder.Services.AddOpenApi();
//builder.Services.AddSwaggerGen();//swagger
#region(swagger)
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ShopApi",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter JWT Token"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference=new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
#endregion

//services
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<ITokenService,TokenService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

//mediatr
builder.Services.AddMediatR(
    cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly)
);

//Redis
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
    options.InstanceName = "ShopApi:";
});
builder.Services.AddScoped<ICacheService,RedisCacheService>();




//piplineBehavior
builder.Services.AddTransient(typeof(IPipelineBehavior<,>),typeof(ValidationBehavior<,>));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>),typeof(LoggingBehavior<,>));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>),typeof(PerformanceBehavior<,>));
#region(authize policy ??????)
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Product.Read", policy =>
    {
        policy.RequireRole("Admin", "Manager");
    });

    options.AddPolicy("Product.Create", policy =>
    {
        policy.RequireRole("Admin");
    });
});
#endregion

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.MapOpenApi();
//}

app.UseHttpsRedirection();

//middele ware
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthentication();
app.UseAuthorization();




app.MapControllers();
app.Run();
