using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MicroProjectApplication.Services.Interfaces;

namespace MicroProjectApplication.Controllers
{
    public class SsoController : ApiController
    {
        private readonly ISsoService _ssoService;
        private readonly IAuditLogger _auditLogger;

        public SsoController(ISsoService ssoService, IAuditLogger auditLogger)
        {
            _ssoService = ssoService;
            _auditLogger = auditLogger;
        }

        [HttpGet]
        [Route("api/sso/auth/start")]
        public IHttpActionResult Start([FromUri] string redirect_uri)
        {
            if (string.IsNullOrEmpty(redirect_uri))
            {
                var errorResponse = _ssoService.CreateErrorResponse("invalid_request");
                _auditLogger.LogRejected("SSO_AUTHZ_REQUEST_REJECTED", "INVALID_REQUEST", null);
                return Content(HttpStatusCode.BadRequest, errorResponse);
            }

            if (!_ssoService.ValidateRedirectUri(redirect_uri, out string errorCode))
            {
                var errorResponse = _ssoService.CreateErrorResponse(errorCode);
                _auditLogger.LogRejected("SSO_AUTHZ_REQUEST_REJECTED", errorCode, redirect_uri);
                return Content(HttpStatusCode.BadRequest, errorResponse);
            }

            return Ok(new { message = "OK", correlation_id = Guid.NewGuid() });
        }

        [HttpGet]
        [Route("api/sso/auth/callback")]
        public IHttpActionResult Callback([FromUri] string redirect_uri)
        {
            if (!Request.IsSecureConnection && !_ssoService.HonorXForwardedProto())
            {
                var errorResponse = _ssoService.CreateErrorResponse("insecure_transport");
                _auditLogger.LogRejected("SSO_CALLBACK_REJECTED", "INSECURE_TRANSPORT", redirect_uri);
                return Content(HttpStatusCode.BadRequest, errorResponse);
            }

            if (!_ssoService.ValidateRedirectUri(redirect_uri, out string errorCode))
            {
                var errorResponse = _ssoService.CreateErrorResponse(errorCode);
                _auditLogger.LogRejected("SSO_CALLBACK_REJECTED", errorCode, redirect_uri);
                return Content(HttpStatusCode.BadRequest, errorResponse);
            }

            return Ok(new { message = "Callback received", correlation_id = Guid.NewGuid() });
        }
    }
}