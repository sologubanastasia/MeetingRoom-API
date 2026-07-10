using System.Net;
using System.Text.Json;

namespace MeetingRoom.WebApi.Middleware
{
    /// <summary>
    /// Глобальний middleware для централізованої обробки винятків.
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Ініціалізує middleware обробки помилок.
        /// </summary>
        /// <param name="next">
        /// Наступний middleware у конвеєрі HTTP-запиту.
        /// </param>
        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Виконує наступний middleware та перехоплює винятки,
        /// які виникають під час обробки HTTP-запиту.
        /// </summary>
        /// <param name="context">Контекст поточного HTTP-запиту.</param>
        /// <returns>Асинхронна операція обробки запиту.</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ArgumentException ex)
            {
                await HandleExceptionAsync(context, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                await HandleExceptionAsync(context, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception)
            {
                await HandleExceptionAsync(
                    context,
                    HttpStatusCode.InternalServerError,
                    "Unexpected server error."
                );
            }
        }

        /// <summary>
        /// Формує стандартну JSON-відповідь з інформацією про помилку.
        /// </summary>
        /// <param name="context">Контекст поточного HTTP-запиту.</param>
        /// <param name="statusCode">HTTP-статус відповіді.</param>
        /// <param name="message">Повідомлення про помилку.</param>
        /// <returns>Асинхронна операція запису відповіді.</returns>
        private static async Task HandleExceptionAsync(
            HttpContext context,
            HttpStatusCode statusCode,
            string message
        )
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var response = new { statusCode = context.Response.StatusCode, message };

            var json = JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(json);
        }
    }
}
