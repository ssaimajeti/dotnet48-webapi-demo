# Mobile Token Storage Guide

## Overview
This document provides implementation guidelines and best practices for secure token storage on mobile platforms (iOS and Android) for Single Sign-On (SSO). The goal is to ensure that sensitive tokens are securely stored using hardware-backed security features available on each platform.

## Key Requirements
- **iOS**: Use Keychain with `kSecAttrAccessibleWhenUnlockedThisDeviceOnly` by default.
- **Android**: Use `EncryptedSharedPreferences` with a `MasterKey` stored in Android Keystore.
- Tokens must not be stored in plaintext or exposed to potentially insecure storage mechanisms.

## iOS Implementation
- **Keychain Storage**:
  - Store access, ID, and refresh tokens using `kSecClassGenericPassword`.
  - Use `kSecAttrAccessibleWhenUnlockedThisDeviceOnly` for accessibility by default.
  - Ensure `kSecAttrSynchronizable` is set to `kCFBooleanFalse` to prevent iCloud sync.

- **Security Considerations**:
  - Avoid storing tokens in `NSUserDefaults`, plist, or any form of file storage.

## Android Implementation
- **Encrypted SharedPreferences**:
  - Use `AndroidX Security Crypto` library.
  - Configure `MasterKey` to use `AES256_GCM` in Android Keystore with StrongBox support.
  - Validate secure hardware using `KeyInfo.isInsideSecureHardware`.

- **Security Considerations**:
  - Block sign-in if hardware-backed keystore is not available.

## Silent Token Refresh
- **Refresh Strategy**:
  - Monitor token `expires_at` and initiate refresh 60–120 seconds before expiry.
  - Use refresh token to obtain new access and ID tokens without user interaction.
  - Requests should be made to the IdP using `grant_type=refresh_token`.

- **Error Handling**:
  - Clear tokens and require re-authentication on unrecoverable errors.
  - Use exponential backoff for network errors, but do not extend token expiration artificially.

## Cryptographic Randomness
- **iOS**: Use `SecRandomCopyBytes` for generating cryptographic keys and nonces.
- **Android**: Use `SecureRandom` from `java.security` for cryptographic randomness.

## Operational & Security Controls
- **Token Redaction**: Do not log tokens; if needed, use HMAC with a non-persistent key.
- **Backup Policy**: Tokens must not be part of device backups or synchronized across devices.


## Regulatory and Compliance Guidelines
- Comply with OWASP MASVS and applicable OAuth 2.0 standards.
- Ensure all storage solutions are vetted and approved by security reviews.

## Risk Assessment
- Devices lacking hardware security modules should be flagged, and user access prevented.
- Time synchronization issues should be mitigated by including buffer time within refresh logic.