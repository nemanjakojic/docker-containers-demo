using code.Core.Operations;

namespace code.Model 
{
    public class SignUpRequest : ServiceRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class SignUpResult : ServiceResponse
    {
    }
}