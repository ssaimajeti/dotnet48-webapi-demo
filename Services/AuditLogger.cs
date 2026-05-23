using System;
using System.Diagnostics;
using MicroProjectApplication.Services.Interfaces;

namespace MicroProjectApplication.Services
{
    public class AuditLogger : IAuditLogger
    {
        private static readonly TraceSource TraceSource = new TraceSource("SsoAuditLog");

        public void LogRejected(string eventType, string reasonCode, string redirectUri)
        {
            var logEntry = new
            {
                timestamp_utc = DateTime.UtcNow.ToString("o"),
                event_type = eventType,
                reason_code = reasonCode,
                redirect_uri = redirectUri,
                client_ip = "TODO", // Extract from request context
                forwarded_proto = "TODO", // Extract or determine from request context
                correlation_id = Guid.NewGuid().ToString()
            };

            TraceSource.TraceEvent(TraceEventType.Warning, 0, 
                Newtonsoft.Json.JsonConvert.SerializeObject(logEntry));
        }
    }
}
