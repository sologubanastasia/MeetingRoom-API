using Abp.MeetingRoom.Bll.Common.RoomBookings.Exceptions;
using Abp.MeetingRoom.Bll.Common.Shared.Exceptions;
using Abp.MeetingRoom.Dal.SqlRepositories.Database.Exceptions;
using Microsoft.Data.SqlClient;
namespace Abp.MeetingRoom.Dal.SqlRepositories.Database.Mappings;
internal static class SqlExceptionTranslator
{
    public static async Task<T> ExecuteAsync<T>(Func<Task<T>> operation)
    {
        try
        {
            return await operation();
        }
        catch (SqlException exception)
        {
            throw Translate(exception);
        }
    }
    private static Exception Translate(SqlException exception)
    {
        var diagnosticDetails = $"SQL error {exception.Number}: {exception.Message}";
        return exception.Number switch
        {
            50010 or 50204 => new BookingConflictException(exception),
            50011 or 50012 => new BusinessRuleException(exception.Message, exception),
            >= 50101 and <= 50303 => new BusinessRuleException(
                exception.Message,
                exception
            ),
            2812 => new DatabaseUnavailableException(
                "The required stored procedure is not installed in the database.",
                diagnosticDetails,
                exception
            ),
            229 => new DatabaseUnavailableException(
                "The database user does not have permission to execute the operation.",
                diagnosticDetails,
                exception
            ),
            -2 or 0 or 5 or 53 or 4060 or 18456 or 11001 =>
                new DatabaseUnavailableException(
                    "The database connection is unavailable.",
                    diagnosticDetails,
                    exception
                ),
            _ => new DatabaseOperationException(
                "A database error occurred.",
                diagnosticDetails,
                exception
            ),
        };
    }
}
