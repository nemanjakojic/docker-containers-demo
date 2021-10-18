using System;
using System.Linq;
using System.Threading.Tasks;
using BCrypt.Net;
using BC = BCrypt.Net.BCrypt;

namespace Docker.Test.Core 
{
    // A Bcrypt-based hash generator.
    public class BCryptHashGenerator : IHashGenerator
    {
        private const HashType DefaultHashType = HashType.SHA384;

        public Task<bool> VerifyHash(string hash, string content)
        {
            return Task.Run(() => 
                BC.Verify(
                    text: content, 
                    hash: hash,
                    hashType: DefaultHashType)
            );
        }
        
        public Task<string> GenerateHash(string content)
        {
            var salt = BC.GenerateSalt(workFactor: 10);
            
            return Task.Run(() => 
                BC.HashPassword(
                    inputKey: content,
                    salt: salt,
                    enhancedEntropy: false, 
                    hashType: DefaultHashType)
            );
        }
    }
}