using Abp.MeetingRoom.Dal.SqlRepositories.Database.Exceptions;
using Microsoft.Data.SqlClient;

namespace Abp.MeetingRoom.Dal.SqlRepositories.Database;

internal static class SqlOperationExecutor
{
    public static async Task<T> ExecuteAsync<T>(Func<Task<T>> operation)
    {
        ArgumentNullException.ThrowIfNull(operation);

        try
        {
            return await operation();
        }
        catch (SqlException exception)
        {
            throw SqlExceptionTranslator.Translate(exception);
        }
    }
}
