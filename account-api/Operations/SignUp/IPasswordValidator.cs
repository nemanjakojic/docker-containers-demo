

using Docker.Test.Core;

namespace Docker.Test.Operations.SignUp 
{
    // Ensures a given password fullfils a predefined password requirements policy.
    public interface IPasswordValidator 
    {
        ValidationResult Validate(string password);
    }
}