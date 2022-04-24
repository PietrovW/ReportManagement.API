using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using MediatR;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using ReportManagement.API.Controllers.V1;
using ReportManagement.API.Extensions;
using ReportManagement.API.OperationFilters;
using ReportManagement.API.OutputFormatters;
using ReportManagement.Application.CommandValidator.V1;
using ReportManagement.Application.Common.V1;
using ReportManagement.Application.Request.V1;
using ReportManagement.Data.Configurations;
using ReportManagement.Domain.Repositorys;
using ReportManagement.Infrastructure.Data;
using ReportManagement.Infrastructure.Data.Settings;
using ReportManagement.Infrastructure.Repositorys;
using Swashbuckle.AspNetCore.SwaggerGen;
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
    options.RespectBrowserAcceptHeader = true; // false by default
    options.OutputFormatters.Add(new XmlSerializerOutputFormatter());

    // register the VcardOutputFormatter
    options.OutputFormatters.Add(new VcardOutputFormatter());
    ////options.RespectBrowserAcceptHeader = true;
    //options.ReturnHttpNotAcceptable = true;
    ////options.Filters.Add<FormatFilter>();
    //options.OutputFormatters.Add(new VcardOutputFormatter());
    //options.OutputFormatters.Add(new ExcelOutputFormatter());
    //options.OutputFormatters.Add(new CsvOutputFormatter());
    //options.FormatterMappings.SetMediaTypeMappingForFormat(
    //                              "csv", "application/csv");
    //options.FormatterMappings.SetMediaTypeMappingForFormat(
    //                            "csv", "text/csv");
}).AddXmlSerializerFormatters()
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});
    //.AddMvcOptions(options =>
    //{
    //    //options.InputFormatters.Add(new PlainTextInputFormatter());
    //    options.OutputFormatters.Add(new CsvOutputFormatter());
    //    options.FormatterMappings.SetMediaTypeMappingForFormat("csv", MediaTypeHeaderValue.Parse("text/csv"));
    //    options.OutputFormatters.Add(new XmlSerializerOutputFormatter());
    //    options.FormatterMappings.SetMediaTypeMappingForFormat("xml", MediaTypeHeaderValue.Parse("application/xml"));
    //});

builder.Services.AddApiVersioning(config =>
{
    config.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    config.AssumeDefaultVersionWhenUnspecified = true;
    config.ReportApiVersions = true;

    config.ApiVersionReader = ApiVersionReader.Combine(
         new QueryStringApiVersionReader("api-version"),
         new HeaderApiVersionReader("X-Version"),
         new MediaTypeApiVersionReader("ver"));
});

builder.Services.AddVersionedApiExplorer(setup =>
{
    setup.GroupNameFormat = "'v'VVV";
    setup.SubstituteApiVersionInUrl = true;
});

builder.Services.AddFluentValidation(fv =>
{
    fv.DisableDataAnnotationsValidation = true;
    fv.AutomaticValidationEnabled = false;
});
builder.Services.Configure<MongoOptions>(
    builder.Configuration.GetSection(MongoOptions.Position));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

//

builder.Services.AddFluentValidationRulesToSwagger();
builder.Services.AddSwaggerGen(options =>
{
    options.OperationFilter<AddCommonParameOperationFilter>();
    options.OperationFilter<SwaggerDefaultValues>();
   // options.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1", Description = "Test Description" });
   // options.SwaggerDoc("v2", new OpenApiInfo { Title = "My API - V2", Version = "v2", Description = "Test Description v2" });
    options.EnableAnnotations();
});

builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();
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
});
//app.MapGet("api/reports/{id}", async (IMediator mediator,Guid id) =>
//{
//    return await mediator.Send(new GetReportQuery() { Id = id });
//});//.Accepts<ReportDto>("text/xml").Accepts<ReportDto>("text/vcard").Accepts<ReportDto>("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
//.Produces<ReportDto>(StatusCodes.Status200OK, "text/xml")
// .Produces(StatusCodes.Status404NotFound);//.Produces(StatusCodes.Status200OK, "application/json", "text/vcard", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

//app.MapPost("api/reports", async (IValidator<CreateReportRequest> validator, IMediator mediator, LinkGenerator links, [FromBody] CreateReportRequest request) =>
//{
//    ValidationResult validationResult = validator.Validate(request);

//    if (!validationResult.IsValid)
//    {
//        return Results.BadRequest(validationResult);
//    }

//    var command = new CreateReportCommand() { Name = request.Name };
//    var result = await mediator.Send(command);
//    return Results.Created($"/reports/{result}", command);
//});
//app.MapDelete("api/reports", async (IMediator mediator, Guid id) =>
//{
//    var command = new DeleteReportCommand() { Id = id };
//    var result = await mediator.Send(command);
//    return Results.Created($"/reports/{result}", command);
//});
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.MigrateDatabase();
app.Run();
