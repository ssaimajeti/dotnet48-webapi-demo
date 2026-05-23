using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using MicroProjectApplication.Services.Interfaces;

namespace MicroProjectApplication.Services
{
    public class SsoService : ISsoService
    {
        private readonly HashSet<string> _allowedRedirectUris;
        private readonly bool _honorXForwardedProto;

        public SsoService()
        {
            _allowedRedirectUris = LoadAllowedRedirectUris();
            _honorXForwardedProto = LoadHonorXForwardedProto();
        }

        public bool ValidateRedirectUri(string redirectUri, out string errorCode)
        {
            errorCode = string.Empty;

            if (string.IsNullOrWhiteSpace(redirectUri) || !Uri.TryCreate(redirectUri, UriKind.Absolute, out Uri uriResult))
            {
                errorCode = "invalid_redirect_uri";
                return false;
            }

            if (uriResult.Scheme != Uri.UriSchemeHttps || !_allowedRedirectUris.Contains(uriResult.Host))
            {
                errorCode = "invalid_redirect_uri";
                return false;
            }

            return true;
        }

        public bool HonorXForwardedProto() => _honorXForwardedProto;

        public ErrorResponse CreateErrorResponse(string errorCode)
        {
            return new ErrorResponse
            {
                Error = errorCode,
                CorrelationId = Guid.NewGuid().ToString(),
                Locale = "en-US"  // Placeholder, better locale resolution needed
                // Fetch localized error description
            };
        }

        private HashSet<string> LoadAllowedRedirectUris()
        {
            var uris = ConfigurationManager.AppSettings["Sso:AllowedRedirectUris"];
            return new HashSet<string>(uris?.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries) ?? Enumerable.Empty<string>());
        }

        private bool LoadHonorXForwardedProto()
        {
            var setting = ConfigurationManager.AppSettings["Sso:HonorXForwardedProto"];
            return bool.TryParse(setting, out bool honorXForwardedProto) && honorXForwardedProto;
        }
    }
}
