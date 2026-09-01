namespace Abp.MeetingRoom.Dal.SqlRepositories.Database.Exceptions;
public sealed class DatabaseUnavailableException : DatabaseAccessException
{
    public DatabaseUnavailableException(
        string message,
        string diagnosticDetails,
        Exception innerException
    )
        : base(message, diagnosticDetails, innerException) { }
}
