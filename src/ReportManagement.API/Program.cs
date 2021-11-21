using FluentValidation.AspNetCore;
using MediatR;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ReportManagement.Infrastructure.Data;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
Assembly[]  assemblie = new Assembly[] { typeof(ReportManagement.Infrastructure.AutoMapper.Profiles).GetTypeInfo().Assembly };
builder.Services.AddControllers()
             .AddJsonOptions(o =>
             {
                 o.JsonSerializerOptions.PropertyNamingPolicy = null;
                 o.JsonSerializerOptions.DictionaryKeyPolicy = null;
             }).AddFluentValidation();
builder.Services.AddMediatR(assemblie);
builder.Services.AddAutoMapper(assemblie);
builder.Services.AddFluentValidation();//.AddFluentValidation(assemblie);
builder.Services.AddFluentValidationRulesToSwagger();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});

builder.Services.AddDbContext<ApplicationDbContext>(options=> options.UseSqlite("Filename=ReportDatabase.db", options =>
{
    options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
}));

var app = builder.Build();
app.UseStaticFiles();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    c.RoutePrefix = string.Empty;
});
app.MapControllers();
app.Run();
