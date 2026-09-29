using Microsoft.AspNetCore.Http;
using System.Text;

namespace HeaderLogger.Middleware
{
    public class HeaderLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public HeaderLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {

            var request = context.Request;

            var log = new StringBuilder();

            log.AppendLine("========================================");
            log.AppendLine($"Дата и время: {DateTime.Now}");
            log.AppendLine($"Метод: {request.Method}");
            log.AppendLine($"Путь: {request.Path}");
            log.AppendLine("HTTP-заголовки:");

            foreach (var header in request.Headers)
            {
                log.AppendLine($"{header.Key}: {header.Value}");
            }

            log.AppendLine();

            string filePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "request-headers.txt"
            );

            await File.AppendAllTextAsync(
                filePath,
                log.ToString(),
                Encoding.UTF8
            );

            await _next(context);
        }
    }
}