using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using MediatR;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ReportManagement.API.Extensions;
using ReportManagement.API.OperationFilters;
using ReportManagement.API.OutputFormatters;
using ReportManagement.API.Request;
using ReportManagement.Application.CommandValidator;
using ReportManagement.Application.Common;
using ReportManagement.Application.Queries;
using ReportManagement.Data.Configurations;
using ReportManagement.Domain.Repositorys;
using ReportManagement.Infrastructure.Data;
using ReportManagement.Infrastructure.Data.Settings;
using ReportManagement.Infrastructure.Repositorys;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<MongoOptions>(
    builder.Configuration.GetSection(MongoOptions.Position));
Assembly[] assemblie = new Assembly[] { typeof(ReportManagement.Application.AutoMapper.Profiles).GetTypeInfo().Assembly };
builder.Services.AddValidatorsFromAssemblyContaining<CreateReportRequest>(lifetime: ServiceLifetime.Scoped);
builder.Services.AddValidatorsFromAssemblyContaining<CreateReportCommand>(lifetime: ServiceLifetime.Scoped);
builder.Services.AddTransient(typeof(IReadBaseRepository<>), typeof(ReadBaseRepository<>));
builder.Services.AddTransient(typeof(IWriteBaseRepository<>), typeof(WriteBaseRepository<>));
builder.Services.AddTransient<IReadReportRepository, ReadReportRepository>();
builder.Services.AddTransient<IWriteReportRepository, WriteReportRepository>();
builder.Services.AddMediatR(assemblie);
builder.Services.AddAutoMapper(assemblie);
builder.Services.AddControllers(options =>
{
    options.RespectBrowserAcceptHeader = true;
    options.ReturnHttpNotAcceptable = true;
    options.Filters.Add<FormatFilter>();
    options.OutputFormatters.Add(new VcardOutputFormatter());
    options.OutputFormatters.Add(new ExcelOutputFormatter());
    options.OutputFormatters.Add(new CsvOutputFormatter());
}).AddXmlSerializerFormatters()
.AddXmlDataContractSerializerFormatters()
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});

builder.Services.AddFluentValidation(fv =>
{
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
    c.OperationFilter<AddCommonParameOperationFilter>();
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    c.EnableAnnotations();
});

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite("Filename=ReportDatabase.db", options =>
 {
     options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
 }));
builder.Services.AddScoped<IApplicationMongoDbContext, ApplicationMongoDbContext>();
var app = builder.Build();
app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    c.RoutePrefix = string.Empty;
});
app.UseReDoc(c =>
{
    c.DocumentTitle = "REDOC API Documentation";
    c.SpecUrl = "/swagger/v1/swagger.json";
    c.DocumentTitle = "My API Docs";
    c.EnableUntrustedSpec();
    c.ScrollYOffset(10);
    c.HideHostname();
    c.HideDownloadButton();
    c.ExpandResponses("200,201");
    c.RequiredPropsFirst();
    c.NoAutoAuth();
    c.PathInMiddlePanel();
    c.HideLoading();
    c.NativeScrollbars();
    c.DisableSearch();
    c.OnlyRequiredInSamples();
    c.SortPropsAlphabetically();
});
app.MapGet("api/reports/{id:Guid}.{format?}", async (IMediator mediator, Guid id) =>
{
    return await mediator.Send(new GetReportQuery() { Id = id });
});//.Produces(StatusCodes.Status200OK, "application/json", "text/vcard", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

app.MapPost("api/reports", async (IValidator<CreateReportRequest> validator, IMediator mediator, LinkGenerator links, [FromBody] CreateReportRequest request) =>
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
app.MapDelete("api/reports", async (IMediator mediator, Guid id) =>
{
    var command = new DeleteReportCommand() { Id = id };
    var result = await mediator.Send(command);
    return Results.Created($"/reports/{result}", command);
});
app.MigrateDatabase();
app.Run();
