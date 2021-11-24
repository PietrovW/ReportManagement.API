using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using MediatR;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ReportManagement.API.Extensions;
using ReportManagement.API.Request;
using ReportManagement.Application.CommandValidator;
using ReportManagement.Application.Common;
using ReportManagement.Application.Queries;
using ReportManagement.Domain.Repositorys;
using ReportManagement.Infrastructure.Data;
using ReportManagement.Infrastructure.Repositorys;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
Assembly[]  assemblie = new Assembly[] { typeof(ReportManagement.Application.AutoMapper.Profiles).GetTypeInfo().Assembly };
builder.Services.AddValidatorsFromAssemblyContaining<CreateReportRequest>(lifetime: ServiceLifetime.Scoped);
builder.Services.AddValidatorsFromAssemblyContaining<CreateReportCommand>(lifetime: ServiceLifetime.Scoped);
builder.Services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddTransient<IReportRepository, ReportRepository>();
builder.Services.AddMediatR(assemblie);
builder.Services.AddAutoMapper(assemblie);
builder.Services.AddFluentValidation(fv => {
    fv.DisableDataAnnotationsValidation = true;
    fv.AutomaticValidationEnabled = false;
});
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

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
app.MapPost("reports", async (IValidator<CreateReportRequest> validator, IMediator mediator, LinkGenerator links, [FromBody] CreateReportRequest request) =>
{
ValidationResult validationResult = validator.Validate(request);

    if (!validationResult.IsValid)
    {
        return Results.BadRequest(validationResult);
    }
    
    var command = new CreateReportCommand() { Name = request.Name };
    var result = await mediator.Send(command);
    return Results.Created($"/reports/{result}", command);
});
app.MigrateDatabase();
app.Run();
