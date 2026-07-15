
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using ShopApi.Application.Behaviors;
using ShopApi.Application.Common.Interfaces;
using ShopApi.Application.Services;
using ShopApi.Infrastructure.Data;
using ShopApi.Infrastructure.Services;
using ShopApi.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
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
//builder.Services.AddOpenApi();
//builder.Services.AddSwaggerGen();//swagger
builder.Services.AddValidatorsFromAssemblyContaining<Program>();//fluentvalidator


//services
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<ITokenService,TokenService>();

//mediatr
builder.Services.AddMediatR(
    cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly)
);

//piplineBehavior
builder.Services.AddTransient(typeof(IPipelineBehavior<,>),typeof(ValidationBehavior<,>));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>),typeof(LoggingBehavior<,>));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>),typeof(PerformanceBehavior<,>));


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
