namespace Abp.MeetingRoom.Dal.SqlRepositories.Database.Exceptions;
public abstract class DatabaseAccessException : Exception
{
    protected DatabaseAccessException(
        string message,
        string diagnosticDetails,
        Exception? innerException = null
    )
        : base(message, innerException)
    {
        DiagnosticDetails = diagnosticDetails;
    }
    public string DiagnosticDetails { get; }
}
