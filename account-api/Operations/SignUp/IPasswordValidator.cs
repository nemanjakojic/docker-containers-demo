using code.Core.Operations;

namespace code.Operations.SignUp 
{
    public interface IPasswordValidator 
    {
        ValidationResult Validate(string password);
    }
}