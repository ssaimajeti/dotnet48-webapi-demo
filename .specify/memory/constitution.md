Principles and guardrails
- Security by default: Tokens and cryptographic material must never be logged, persisted in plaintext, or exposed to inter-process channels. Prefer deny/abort over insecure fallback.
- Hardware-backed storage: iOS Keychain and Android Keystore/EncryptedSharedPreferences are the only allowed persistence for access/id/refresh tokens. If hardware-backed security is not available or cannot be attested, block sign-in and surface a clear, actionable error.
- Zero-trust client posture: Treat the surrounding OS as potentially hostile. Minimize token lifetime in memory; overwrite buffers after use when possible.
- Least privilege and isolation: Use “ThisDeviceOnly” semantics on iOS. Disable iCloud Keychain sync of tokens. Use app-private storage on Android; never use world-readable modes.
- Standards, not custom crypto: Use platform primitives and well-vetted libraries (e.g., AppAuth). No home-grown encryption or token parsing beyond standard validation steps.
- Entropy and randomness: PKCE verifier, state, and nonce require >=128 bits of entropy (we will use 256 bits). Use SecRandomCopyBytes (iOS) and java.security.SecureRandom (Android).
- Compliance baselines: OWASP MASVS (MASVS-STORAGE-1/2, MASVS-CRYPTO-1), NIST SP 800-63 OAuth guidance for refresh flows, and relevant platform secure storage guidance.
- Observability without secrets: Telemetry must never include raw tokens. Redact or hash with keyed HMAC if correlation is required.
- Backups and device migration: Tokens must not be included in cloud or device backups. New device installs require fresh authentication.
- Time and refresh hygiene: Proactively refresh access tokens before expiry; clear tokens on refresh failure.
- Code review expectations: Security reviewer approval is mandatory. Verify secure storage attributes, RNG usage, error handling, and test coverage for edge cases.
- Threat modeling: Document STRIDE risks for token theft, downgrade to insecure storage, insecure RNG, and logging leaks; list mitigations.

Non-functional requirements
- Performance: Token store operations <50 ms p50 local on modern devices; refresh scheduled off the UI thread.
- Reliability: Silent refresh should succeed >=99.5% under normal network conditions; exponential backoff with jitter.
- Privacy: Do not collect PII beyond necessary telemetry, and never collect tokens.
- Accessibility: Errors about insecure device configuration must be understandable and localizable.

Change management and documentation
- Maintain an ADR documenting secure-storage choices and trade-offs.
- Keep specs and implementation notes versioned alongside the code; update README to link to these specs.
- Security test results (SAST, DAST, MobSF) are artifacts of the release.