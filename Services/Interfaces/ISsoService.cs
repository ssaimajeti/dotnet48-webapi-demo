namespace MicroProjectApplication.Services.Interfaces
{
    public interface ISsoService
    {
        bool ValidateRedirectUri(string redirectUri, out string errorCode);
        bool HonorXForwardedProto();
        ErrorResponse CreateErrorResponse(string errorCode);
        // Additional methods for normalization etc.
    }
}
