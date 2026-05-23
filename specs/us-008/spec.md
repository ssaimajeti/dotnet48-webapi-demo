WHAT
Add server-side controls to enforce:
1) Redirect URI whitelist validation for SSO authorization request construction and callback handling.
2) HTTPS-only acceptance for SSO callback requests.

Introduce a new SSO controller with two endpoints:
- GET /api/sso/auth/start
  - Purpose: construct/initiate authorization request (US-002 dependency). In this story, the focus is to validate the provided redirect_uri against the server whitelist and return an i18n-ready error if invalid. If valid, return a placeholder OK payload (no IdP call in this story).
- GET /api/sso/auth/callback
  - Purpose: receive SSO callback. Enforce HTTPS-only callback and validate the redirect_uri against whitelist. Reject non-HTTPS or non-whitelisted URIs with i18n-ready error payloads and audit logs.

Introduce configuration keys in Web.config:
- appSettings:
  - Sso:AllowedRedirectUris: semicolon-separated list of absolute HTTPS URIs
  - Sso:HonorXForwardedProto: boolean, default false
- system.diagnostics listeners (optional) to write audit logs to App_Data/Logs/sso-audit.log via TextWriterTraceListener.

Error payload contract (JSON):
- error: stable error code (e.g., invalid_redirect_uri, insecure_transport, invalid_request, invalid_configuration)
- error_description: localized user-facing message from Resources/Errors.resx
- correlation_id: GUID for traceability
- locale: the resolved locale (e.g., en-US)

Audit events (structured, one line JSON per event):
- event_type: SSO_CALLBACK_REJECTED or SSO_AUTHZ_REQUEST_REJECTED
- reason_code: INVALID_REDIRECT_URI or INSECURE_TRANSPORT or INVALID_CONFIGURATION
- redirect_uri: value received (if present)
- client_ip, forwarded_proto, correlation_id, timestamp_utc

WHY
- Prevent open redirect vulnerabilities and token leakage by allowing only pre-registered redirect URIs.
- Ensure confidentiality and integrity by disallowing callbacks over unencrypted HTTP.
- Provide operational visibility and localized error messages for better UX and compliance.

Acceptance criteria
1) Whitelist enforcement on /api/sso/auth/start:
   - When redirect_uri is missing, response is 400 with error=invalid_request and localized description; audit event SSO_AUTHZ_REQUEST_REJECTED reason INVALID_REQUEST.
   - When redirect_uri is not an absolute URI or not HTTPS, response is 400 invalid_redirect_uri; audit emitted.
   - When redirect_uri is absolute HTTPS but not in whitelist, response is 400 invalid_redirect_uri; audit emitted.
   - When redirect_uri is in whitelist, response is 200 with a placeholder success JSON indicating it would proceed (no external redirect in this story).

2) HTTPS-only and whitelist on /api/sso/auth/callback:
   - If the request is received via HTTP (Request.IsSecureConnection=false) and HonorXForwardedProto=false, return 400 insecure_transport; audit emitted.
   - If HonorXForwardedProto=true and X-Forwarded-Proto != https, return 400 insecure_transport; audit emitted.
   - If redirect_uri missing or invalid absolute HTTPS URI: 400 invalid_redirect_uri; audit emitted.
   - If redirect_uri not in whitelist: 400 invalid_redirect_uri; audit emitted.
   - If both checks pass: response is 200 with a placeholder success JSON indicating receipt.

3) Normalization and matching:
   - Scheme and host case-insensitive.
   - Default ports removed (443 for https).
   - Trailing slash on path ignored for matching.
   - Query must match exactly if present on the whitelisted entry.

4) Localization:
   - Errors are returned in the language best matching Accept-Language header if a corresponding .resx is provided; otherwise default en.
   - At least the following keys exist in Resources/Errors.resx: Errors_InvalidRedirectUri, Errors_InsecureTransport, Errors_InvalidRequest, Errors_InvalidConfiguration.

5) Audit logging:
   - Each rejection emits a single structured log entry with fields listed above.
   - No stack traces or sensitive tokens in logs.

6) Configuration behavior:
   - Empty or missing Sso:AllowedRedirectUris causes all requests requiring whitelist to be rejected with invalid_configuration; audit emitted.

Out of scope
- Full OAuth/OIDC redirection to and from an actual IdP.
- Token processing, user session creation, CSRF/state parameter validation.
- Global HTTPS enforcement for all endpoints.

Cross-service dependencies
- None at runtime for this story. Operational dependency on IIS/reverse proxy to set X-Forwarded-Proto accurately when HonorXForwardedProto=true.