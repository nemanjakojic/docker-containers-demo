using System.Threading.Tasks;

namespace code.Core
{
    public interface IHashGenerator 
    {
        // Generates a cryptographically-strong hash from a given text value.
        Task<string> GenerateHash(string content);

        // Checks whether a given hash mathes a given content.
        Task<bool> VerifyHash(string hash, string content);
    }
}