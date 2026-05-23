WHAT
Implement platform-specific, hardware-backed secure token storage for mobile SSO:
- iOS: Store access token, ID token, and refresh token in Keychain with “ThisDeviceOnly” and non-synchronizable attributes. Use appropriate kSecAttrAccessible for app UX (default WhenUnlockedThisDeviceOnly; AfterFirstUnlockThisDeviceOnly only if background refresh is required).
- Android: Store tokens using AndroidX Security Crypto’s EncryptedSharedPreferences with a MasterKey whose key material lives in Android Keystore; prefer StrongBox-backed keys when available; verify hardware backing. Block sign-in if hardware-backed keystore is not available or cannot be verified.
- Silent refresh: Before access token expiry, use refresh token to obtain new tokens and atomically update secure storage. Clear tokens on unrecoverable failures.
- Cryptographic randomness: Generate PKCE verifier, state, and nonce with >=128-bit entropy using SecRandomCopyBytes (iOS) and SecureRandom (Android). We will use 256 bits.

WHY
- Prevent token disclosure via filesystem, backups, or inter-app access.
- Comply with OWASP MASVS and modern OAuth 2.0 for native apps guidance.
- Reduce user friction via silent refresh, minimizing interactive logins.

User story narrative
As a mobile user, after successful SSO login, my tokens are protected by the device’s hardware security module so that even if another app or a rooted/jailbroken tool inspects storage, my tokens are not retrievable. When my access token is near expiry, the app refreshes it automatically without asking me to sign in again, unless the device is insecure or offline.

Acceptance criteria
Secure storage
- iOS
  - Tokens are stored using Keychain kSecClassGenericPassword with:
    - kSecAttrAccessible = kSecAttrAccessibleWhenUnlockedThisDeviceOnly (or AfterFirstUnlockThisDeviceOnly only when background refresh is a hard requirement and approved by security).
    - kSecAttrSynchronizable = kCFBooleanFalse.
    - kSecAttrAccessGroup limited to the app’s default (no shared group unless explicitly approved).
  - Keychain queries use kSecReturnData=false unless needed, minimize read access.
  - No tokens in NSUserDefaults, plist, files, logs, pasteboard, or URL caches.
- Android
  - Use EncryptedSharedPreferences with MasterKey built as AES256_GCM master key stored in Android Keystore.
  - Prefer StrongBox-backed keys when available; otherwise verify KeyInfo.isInsideSecureHardware is true. If not hardware-backed, block login with an explicit error and do not persist tokens.
  - Never store tokens in standard SharedPreferences, files, or logs.
  - SharedPreferences mode is private (Context.MODE_PRIVATE); no world-readable modes.
Silent refresh
- Token manager tracks expires_at and schedules refresh 60–120 seconds before expiry, accounting for clock skew.
- Refresh request:
  - POST to the IdP token endpoint with grant_type=refresh_token and client authentication as per app registration.
  - On success, atomically replace tokens in secure storage. On failure due to invalid_grant, clear tokens and require user re-authentication.
  - Network and server errors: retry with capped exponential backoff; never extend token lifetime artificially.
Entropy
- PKCE verifier: 32 random bytes, base64url (no padding) to 43–128 chars as RFC 7636-compliant.
- state and nonce: 16–32 random bytes minimum; we will use 32 bytes.
- RNG: SecRandomCopyBytes (iOS), SecureRandom.getInstanceStrong() or default SecureRandom (Android); no Math.random.
Operational/security controls
- Logging redacts tokens; telemetry uses event names without secrets. If correlation is needed, use HMAC(token_id) with a rotating key not persisted on-device.
- Tokens are excluded from backups and not synchronized across devices.
- Jailbreak/root detection does not weaken storage guarantees; do not switch to insecure storage under any condition.
- Unit and instrumentation tests validate storage attributes, hardware backing checks, refresh scheduling, and redaction.

Out of scope
- Changes to the identity provider configuration or server-side token issuance.
- Backend API changes in ssaimajeti/dotnet48-webapi-demo (this repo has no mobile code).
- Biometric-gated access to tokens (may be considered later).
- Multi-app Keychain/Keystore sharing.

Cross-service dependencies
- Identity Provider (IdP): OAuth 2.0/OIDC token endpoint for refresh.
- Mobile CI/CD: Signing configs, ProGuard/R8, Swift toolchain, AndroidX Security Crypto.
- Security review: AppSec team sign-off on storage attributes, RNG, and refresh logic.
- This repository: Documentation and integration guidance only; no runtime code changes.

Risk and mitigations
- Device without hardware-backed keystore: block sign-in and surface error.
- Time skew causes premature expiry: refresh window set with skew buffer.
- Token leakage via crash logs: do not include tokens in exceptions; add log scrubbing.
- Background refresh limits on iOS: choose appropriate Keychain accessibility; if background not possible, refresh on foreground with grace handling.