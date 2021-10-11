using Array.Test.Core;

namespace Array.Test.Operations.SignUp 
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