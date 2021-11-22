using FluentValidation.AspNetCore;
using MediatR;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ReportManagement.API.Extensions;
using ReportManagement.API.Request;
using ReportManagement.Application.Common;
using ReportManagement.Application.Queries;
using ReportManagement.Domain.Repositorys;
using ReportManagement.Infrastructure.Data;
using ReportManagement.Infrastructure.Repositorys;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
Assembly[]  assemblie = new Assembly[] { typeof(ReportManagement.Application.AutoMapper.Profiles).GetTypeInfo().Assembly };
builder.Services.AddControllers()
             .AddJsonOptions(o =>
             {
                 o.JsonSerializerOptions.PropertyNamingPolicy = null;
                 o.JsonSerializerOptions.DictionaryKeyPolicy = null;
             }).AddFluentValidation();


builder.Services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddTransient<IReportRepository, ReportRepository>();
builder.Services.AddMediatR(assemblie); 
builder.Services.AddAutoMapper(assemblie);
builder.Services.AddFluentValidation();
builder.Services.AddFluentValidationRulesToSwagger();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});

builder.Services.AddDbContext<ApplicationDbContext>(options=> options.UseSqlite("Filename=ReportDatabase5.db", options =>
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
app.MapGet("/reports/{id}", async  (IMediator mediator, Guid id) =>
{
    return await mediator.Send(new GetReportQuery() { Id = id });
});
app.MapPost("reports", async (HttpContext httpContext, IMediator mediator, LinkGenerator links, CreateReportRequest request) =>
{
    var command = new CreateReportCommand() { Name = request.Name };
    var result = await mediator.Send(command);
    return Results.Created($"/reports/{result}", command);
});
app.MapControllers();
app.MigrateDatabase();
app.Run();
