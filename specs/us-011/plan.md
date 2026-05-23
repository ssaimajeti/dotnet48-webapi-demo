Delivery approach
- This is a cross-platform mobile security implementation with docs and integration guidance committed to ssaimajeti/dotnet48-webapi-demo. No API/controller changes are required in this repo; it will host the specification, acceptance criteria, and references consumed by mobile teams.
- Implement token stores as thin, audited wrappers around platform primitives:
  - iOS TokenStore (Swift): Keychain-backed CRUD with ThisDeviceOnly and non-synchronizable flags; atomic update semantics; memory zeroization where feasible.
  - Android TokenStore (Kotlin): EncryptedSharedPreferences with Keystore MasterKey (AES256_GCM), StrongBox if available; KeyInfo verification; atomic apply/commit.

Key architecture decisions
- Use AppAuth (iOS and Android) for OAuth/OIDC flows to avoid rolling our own PKCE and token exchange. Our TokenStore plugs into AppAuth’s token storage hooks.
- Block login on non-hardware-backed keystore to meet the “exclusively hardware-backed” requirement; provide a user-facing message and remediation guidance.
- Access token lifetime and refresh window: refresh 90 seconds before expiry with jitter ±30 seconds to spread load; three retries (200ms, 1s, 5s) with jitter, then fail and clear tokens.

API contract (IdP token endpoint)
- Request: POST {token_endpoint}
  - Headers: Content-Type: application/x-www-form-urlencoded; Accept: application/json
  -