namespace MicroProjectApplication.Models.Responses
{
    public class ErrorResponse
    {
        public string Error { get; set; }
        public string ErrorDescription { get; set; }
        public string CorrelationId { get; set; }
        public string Locale { get; set; }
    }
}
