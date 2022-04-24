using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using ReportManagement.Application.Dtos.V1;
using System.Text;

namespace ReportManagement.API.OutputFormatters
{
    public class CsvOutputFormatter : TextOutputFormatter
    {
        public CsvOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/csv"));

            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        protected override bool CanWriteType(Type? type)
         => typeof(ReportDto).IsAssignableFrom(type)
             || typeof(IEnumerable<ReportDto>).IsAssignableFrom(type);
        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var response = context.HttpContext.Response;
            var buffer = new StringBuilder();
            if (context.Object is IEnumerable<ReportDto>)
            {
                foreach (var Blog in (IEnumerable<ReportDto>)context.Object)
                {
                    FormatCsv(buffer, Blog);
                }
            }
            else
            {
                FormatCsv(buffer, (ReportDto)context.Object);
            }
            await response.WriteAsync(buffer.ToString(), selectedEncoding);
        }
        private static void FormatCsv(StringBuilder buffer, ReportDto report)
        {
           
                buffer.AppendLine($"{report.Name},\"{report.Id}\"");
            
        }
    }
}
