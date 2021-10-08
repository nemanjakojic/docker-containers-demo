using System.Linq;
using System.Text;
using System.Threading.Tasks;
using code.Core.Operations;
using code.Data;
using code.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace code.Core.Application {

    public class SignUpOperation : Operation<SignUpRequest, SignUpResult>
    {
        private readonly AppDbContext _context;
        private readonly IHashGenerator _hashGenerator;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        public SignUpOperation(
            ILogger<SignUpOperation> logger,
            AppDbContext context,
            IHashGenerator hashGenerator,
            IDateTimeProvider dateTimeProvider, 
            IHttpContextAccessor httpContextAccessor) : base(logger)
        {
            _context = context;
            _hashGenerator = hashGenerator;
            _dateTimeProvider = dateTimeProvider;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override ValidationResult ValidateRequest(SignUpRequest request) 
        {
            // Sanitize input data
            request.Username = request.Username?.Trim();
            request.Password = request.Password?.Trim();

            if (string.IsNullOrWhiteSpace(request.Username)) 
            {
                return ValidationResult.Failure().WithMessage("Unable to sign up - invalid username.");
            }

            if (string.IsNullOrWhiteSpace(request.Password)) 
            {
                return ValidationResult.Failure().WithMessage("Unable to sign up - invalid password.");
            }
            
            return ValidationResult.Success();
        }

        protected override async Task<SignUpResult> ExecuteRequest(SignUpRequest request, ValidationResult validationResult)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            
            // Check if the account already exists
            var accountExists = _context.Account.Any(a => a.Username == request.Username);
            if (accountExists) 
            {
                return new SignUpResult { Success = false, Message = "Unable to sign up - account already exists." };
            }

            var now = _dateTimeProvider.GetUtcNow();
            var passwordHash = await _hashGenerator.GenerateHash(request.Password);

            await _context.Account.AddAsync(new Data.Entity.Account
            {
                Username = request.Username.Trim(),
                PasswordHash = Encoding.ASCII.GetBytes(passwordHash),
                Created = now,
                CreatedBy = "app",
                Modified = now,
                ModifiedBy = "app"
            });

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return new SignUpResult { Success = true, Message = "Account registered successfully." };
        }
    }
}