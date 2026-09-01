namespace Abp.MeetingRoom.Dal.SqlRepositories.Database.Exceptions;
public sealed class DatabaseOperationException : DatabaseAccessException
{
    public DatabaseOperationException(
        string message,
        string diagnosticDetails,
        Exception? innerException = null
    )
        : base(message, diagnosticDetails, innerException) { }
}
