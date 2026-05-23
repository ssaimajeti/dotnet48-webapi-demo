Architecture and approach
- Add SsoController with two actions:
  - Start (GET /api/sso/auth/start): validates redirect_uri via ISsoService; emits audit on rejection; returns success JSON on acceptance.
  - Callback (GET /api/sso/auth/callback): enforces HTTPS (IsSecureConnection or X-Forwarded-Proto), validates redirect_uri via ISsoService; emits audit; returns success JSON on acceptance.
- Add ISsoService and SsoService implementing:
  - Normalization of URIs.
  - Cached HashSet<string> of allowed normalized URIs loaded from Web.config at construction.
  - Validation methods: ValidateRedirectUri(string raw, out string normalized, out string errorCode).
- Add IAuditLogger and AuditLogger that writes structured JSON to System.Diagnostics.Trace. Configure TextWriterTraceListener in Web.config to write to App_Data/Logs/sso-audit.log.
- Add DTOs/Models:
  - ErrorResponse with error, error_description, correlation_id, locale.
  - SuccessResponse with message and correlation_id.
- Add Resources/Errors.resx (and an example secondary locale, e.g., Errors.es.resx) with required keys.
- Wire dependencies in UnityConfig: register ISsoService and IAuditLogger.

API contracts
- GET /api/sso/auth/start?redirect_uri={absolute-https-uri}
  - 200: { "message": "OK", "correlation_id": "..." }
  - 400: { "error": "...", "error_description": "...", "correlation_id": "...", "locale": "..." }
- GET /api/sso/auth/callback?redirect_uri={absolute-https-uri}
  - 200 or 400 as above, with additional HTTPS enforcement.

Configuration changes (Web.config)
- appSettings:
  - <add key="Sso:AllowedRedirectUris" value="https://app.example.com/callback;https://portal.example.com/auth/cb" />
  - <add key="Sso:HonorXForwardedProto" value="false" />
- system.diagnostics (optional but recommended):
  - Add TextWriterTraceListener writing to App_Data/Logs/sso-audit.log.

I18n handling
- Resolve culture from Accept-Language; fall back to InvariantCulture/en.
- Use Resources.Errors.ResourceManager.GetString(key, culture) to produce error_description.

Files/classes to add or modify
- Add Controllers/SsoController.cs
- Add Services/Interfaces/