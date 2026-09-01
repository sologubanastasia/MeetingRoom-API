using System.Net;
using Abp.MeetingRoom.Bll.Common.RoomBookings.Exceptions;
using Abp.MeetingRoom.Bll.Common.Shared.Exceptions;
using Abp.MeetingRoom.Dal.SqlRepositories.Database.Exceptions;
namespace Abp.MeetingRoom.Services.Web.Middleware;
public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly IWebHostEnvironment _environment;
    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger,
        IWebHostEnvironment environment
    )
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested)
        {
            _logger.LogInformation(
                "Request {TraceId} was cancelled by the client.",
                context.TraceIdentifier
            );
        }
        catch (Exception exception)
        {
            if (context.Response.HasStarted)
            {
                _logger.LogError(
                    exception,
                    "Cannot handle exception because the response has already started."
                );
                throw;
            }
            var (statusCode, message) = MapException(exception);
            LogException(context, exception, statusCode);
            await HandleExceptionAsync(context, statusCode, message);
        }
    }
    private (HttpStatusCode StatusCode, string Message) MapException(Exception exception)
    {
        return exception switch
        {
            ArgumentException => (HttpStatusCode.BadRequest, exception.Message),
            InvalidOperationException => (HttpStatusCode.BadRequest, exception.Message),
            BookingConflictException =>
                (
                    HttpStatusCode.BadRequest,
                    "Meeting room is already booked for this time."
                ),
            BusinessRuleException => (HttpStatusCode.BadRequest, exception.Message),
            DatabaseUnavailableException databaseException =>
                (
                    HttpStatusCode.ServiceUnavailable,
                    GetDatabaseMessage(databaseException)
                ),
            DatabaseOperationException databaseException =>
                (
                    HttpStatusCode.InternalServerError,
                    GetDatabaseMessage(databaseException)
                ),
            TimeoutException or OperationCanceledException =>
                (
                    HttpStatusCode.ServiceUnavailable,
                    "The operation timed out. Please try again later."
                ),
            _ => (HttpStatusCode.InternalServerError, "Unexpected server error."),
        };
    }
    private string GetDatabaseMessage(DatabaseAccessException exception)
    {
        return _environment.IsDevelopment()
            ? $"{exception.Message} {exception.DiagnosticDetails}"
            : exception.Message;
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
    private static async Task HandleExceptionAsync(
        HttpContext context,
        HttpStatusCode statusCode,
        string message
    )
    {
        context.Response.StatusCode = (int)statusCode;
        await context.Response.WriteAsJsonAsync(
            new
            {
                statusCode = context.Response.StatusCode,
                message,
                traceId = context.TraceIdentifier,
            }
        );
    }
}
