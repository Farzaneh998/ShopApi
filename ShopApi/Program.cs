
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using ShopApi.Application.Behaviors;
using ShopApi.Application.Services;
using ShopApi.Infrastructure.Data;
using ShopApi.Middlewares;


//serilog config
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Seq("http://localhost:5341")
    .CreateLogger();


var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();//useserilog insted  asp Ilogger


builder.Services.AddControllers();
builder.Services.AddDbContext<ShopDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"));
});


builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();//swagger
builder.Services.AddValidatorsFromAssemblyContaining<Program>();//fluentvalidator

//services
builder.Services.AddScoped<ProductService>();



//mediatr
builder.Services.AddMediatR(
    cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly)
);

//piplineBehavior
builder.Services.AddTransient(typeof(IPipelineBehavior<,>),typeof(ValidationBehavior<,>));



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
app.UseAuthorization();




app.MapControllers();
app.Run();
