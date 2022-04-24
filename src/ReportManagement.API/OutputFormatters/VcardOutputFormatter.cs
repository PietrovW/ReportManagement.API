using System.Text;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using ReportManagement.Application.Dtos;
using ReportManagement.Application.Dtos.V1;

namespace ReportManagement.API.OutputFormatters
{
    public class VcardOutputFormatter : TextOutputFormatter
    {
        public VcardOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/plain"));

            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        protected override bool CanWriteType(Type? type)
            => typeof(ReportDto).IsAssignableFrom(type)
                || typeof(IEnumerable<ReportDto>).IsAssignableFrom(type);

        public override async Task WriteResponseBodyAsync(
            OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var httpContext = context.HttpContext;
            var serviceProvider = httpContext.RequestServices;

            var logger = serviceProvider.GetRequiredService<ILogger<VcardOutputFormatter>>();
            var buffer = new StringBuilder();

            if (context.Object is IEnumerable<ReportDto> contacts)
            {
                foreach (var contact in contacts)
                {
                    FormatVcard(buffer, contact, logger);
                }
            }
            else
            {
                FormatVcard(buffer, (ReportDto)context.Object!, logger);
            }

            await httpContext.Response.WriteAsync(buffer.ToString(), selectedEncoding);
        }

        private static void FormatVcard(
            StringBuilder buffer, ReportDto contact, ILogger logger)
        {
            buffer.AppendLine("BEGIN:VCARD");
            buffer.AppendLine("VERSION:2.1");
            buffer.AppendLine($"Name:{contact.Name} {contact.Name}");
            buffer.AppendLine($"UID:{contact.Id}");
            buffer.AppendLine("END:VCARD");

            logger.LogInformation("Writing {Name}",
                contact.Name);
        }
    }
}
