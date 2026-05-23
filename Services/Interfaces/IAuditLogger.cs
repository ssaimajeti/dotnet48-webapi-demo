namespace MicroProjectApplication.Services.Interfaces
{
    public interface IAuditLogger
    {
        void LogRejected(string eventType, string reasonCode, string redirectUri);
    }
}
