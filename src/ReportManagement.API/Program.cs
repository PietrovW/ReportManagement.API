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
using ReportManagement.Data.Configurations;
using ReportManagement.Domain.Repositorys;
using ReportManagement.Infrastructure.Data;
using ReportManagement.Infrastructure.Data.Configurations;
using ReportManagement.Infrastructure.Data.Settings;
using ReportManagement.Infrastructure.Repositorys;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
Assembly[]  assemblie = new Assembly[] { typeof(ReportManagement.Application.AutoMapper.Profiles).GetTypeInfo().Assembly };
builder.Services.AddValidatorsFromAssemblyContaining<CreateReportRequest>(lifetime: ServiceLifetime.Scoped);
builder.Services.AddValidatorsFromAssemblyContaining<CreateReportCommand>(lifetime: ServiceLifetime.Scoped);
builder.Services.AddTransient(typeof(IReadBaseRepository<>), typeof(ReadBaseRepository<>));
builder.Services.AddTransient(typeof(IWriteBaseRepository<>), typeof(WriteBaseRepository<>));
builder.Services.AddTransient<IReadReportRepository, ReadReportRepository>();
builder.Services.AddTransient<IWriteReportRepository, WriteReportRepository>();
builder.Services.AddMediatR(assemblie);
builder.Services.AddAutoMapper(assemblie);
builder.Services.AddFluentValidation(fv => {
    fv.DisableDataAnnotationsValidation = true;
    fv.AutomaticValidationEnabled = false;
});
builder.Services.Configure<MongoOptions>(
    builder.Configuration.GetSection(MongoOptions.Position));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.Services.AddFluentValidationRulesToSwagger();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});

builder.Services.AddDbContext<ApplicationDbContext>(options=> options.UseSqlite("Filename=ReportDatabase.db", options =>
{
    options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
}));
builder.Services.AddScoped<IApplicationMongoDbContext, ApplicationMongoDbContext>();
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
