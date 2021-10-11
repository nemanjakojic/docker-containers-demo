using Array.Test.Core;

namespace Array.Test.Operations.LogIn
{
    public class LogInRequest : ServiceRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LogInResult : ServiceResponse
    {
    }
}