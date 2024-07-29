namespace Talabat.APIs.Errors
{
    public class ApiValidationResponse : ApiResponse
    {
        public IEnumerable<string> Error { get; set; }
        public ApiValidationResponse() : base(400)
        {
            Error = new List<string>();
        }
    }
}
