using code.Core.Operations;

namespace code.Model 
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