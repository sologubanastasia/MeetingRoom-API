using System.Net;
using Microsoft.EntityFrameworkCore;

namespace MeetingRoom.WebApi.Middleware
{
    /// <summary>
    /// Глобальний middleware для централізованої обробки винятків.
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        /// <summary>
        /// Ініціалізує middleware обробки помилок.
        /// </summary>
        /// <param name="next">
        /// Наступний middleware у конвеєрі HTTP-запиту.
        /// </param>
        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger
        )
        {
            _next = next;
            _logger = logger;
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
            catch (OperationCanceledException)
                when (context.RequestAborted.IsCancellationRequested)
            {
                _logger.LogInformation(
                    "Request {TraceId} was cancelled by the client.",
                    context.TraceIdentifier
                );
            }
            catch (Exception ex)
            {
                if (context.Response.HasStarted)
                {
                    _logger.LogError(
                        ex,
                        "Cannot handle exception because the response has already started."
                    );

                    throw;
                }

                var (statusCode, message) = ex switch
                {
                    ArgumentException => (HttpStatusCode.BadRequest, ex.Message),
                    InvalidOperationException => (HttpStatusCode.BadRequest, ex.Message),
                    DbUpdateConcurrencyException =>
                        (
                            HttpStatusCode.Conflict,
                            "The resource was modified by another request."
                        ),
                    DbUpdateException =>
                        (
                            HttpStatusCode.InternalServerError,
                            "A database error occurred."
                        ),
                    TimeoutException or OperationCanceledException =>
                        (
                            HttpStatusCode.ServiceUnavailable,
                            "The operation timed out. Please try again later."
                        ),
                    _ =>
                        (
                            HttpStatusCode.InternalServerError,
                            "Unexpected server error."
                        ),
                };

                LogException(context, ex, statusCode);

                await HandleExceptionAsync(context, statusCode, message);
            }
        }

        private void LogException(
            HttpContext context,
            Exception exception,
            HttpStatusCode statusCode
        )
        {
            if ((int)statusCode >= 500)
            {
                _logger.LogError(
                    exception,
                    "Request {TraceId} failed with status code {StatusCode}.",
                    context.TraceIdentifier,
                    (int)statusCode
                );

                return;
            }

            _logger.LogWarning(
                exception,
                "Request {TraceId} failed with status code {StatusCode}.",
                context.TraceIdentifier,
                (int)statusCode
            );
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
            context.Response.StatusCode = (int)statusCode;

            var response = new
            {
                statusCode = context.Response.StatusCode,
                message,
                traceId = context.TraceIdentifier,
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
