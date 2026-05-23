Security-first principles for SSO redirect/callback handling

Core quality principles
- Enforce least-privilege and fail-safe defaults: reject by default unless redirect URI is explicitly whitelisted and transport is HTTPS.
- Deterministic validation: normalization then exact-match comparison; no implicit wildcarding.
- Transparent observability: every rejection must produce structured audit logs without leaking secrets.
- Internationalization-ready: error payloads must be key-driven and localizable using .resx resources; no hard-coded user-facing text in controllers/services.
- Backward-compatible and additive: new endpoints and services must not break existing APIs; existing controllers remain untouched.

Coding standards
- .NET Framework 4.8 Web API conventions; use dependency injection via Unity for new services.
- Keep controllers thin; move validation and policy logic into services.
- Use strongly-typed models for responses; consistent envelope for errors.
- Avoid static mutable state; configuration read once and injected; cache immutable whitelist.
- Input normalization:
  - Parse redirect_uri as absolute URI; reject relative URIs.
  - Normalize by trimming, lowercasing scheme/host, removing default ports, collapsing trailing slashes on path.
- Comparison policy: exact match after normalization including scheme, host, port, and path; query must also match if present.

Architecture guardrails
- HTTPS-only for callback: check Request.IsSecureConnection and honor X-Forwarded-Proto=https when behind trusted proxy (configurable).
- Configuration-driven whitelist: read semicolon-separated list from Web.config appSettings Sso:AllowedRedirectUris.
- Centralized audit logger abstraction (IAuditLogger) to encapsulate sinks (System.Diagnostics listeners). Do not write PII beyond IP, user agent, redirect_uri, and reason code.
- Structured audit fields: timestamp (UTC), event_type, reason_code, redirect_uri, client_ip, forwarded_proto, correlation_id.

Non-functional requirements
- Performance: whitelist lookup O(1) via HashSet after normalization; negligible overhead per request.
- Reliability: if configuration missing or malformed, reject with invalid_configuration and log at error level.
- Localization: default locale en; respect Accept-Language best-match where possible.
- DevOps: configuration keys documented in README; safe defaults (empty whitelist rejects all).
- Testability: service-level unit tests for normalization and matching; controller-level tests for HTTP 400 responses. If unit tests cannot be added, provide manual verification steps.

Review standards and stakeholder expectations
- Security review: validate normalization rules against open redirect risks; confirm HTTPS enforcement works behind reverse proxy as configured.
- Product review: verify error messages are user-appropriate and localized; ensure no personally sensitive data in logs.
- Ops review: confirm diagnostics listeners write to file in App_Data/Logs or preferred sink; rotation and permissions acceptable.
- Acceptance: all criteria met, negative paths covered, and README updated with configuration and example usage.