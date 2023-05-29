namespace WhoWantsMillionApp.Services.Exceptions
{
    public abstract class BaseServiceException : Exception
    {
        public int StatusCode { get; set; }
        public BaseServiceException(string message, int statusCode) : base(message)
        {
            this.StatusCode = statusCode;
        }
    }
}
