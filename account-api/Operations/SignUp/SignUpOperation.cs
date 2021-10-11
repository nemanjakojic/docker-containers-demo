using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Array.Test.Core;
using Array.Test.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Array.Test.Operations.SignUp 
{
    public class SignUpOperation : Operation<SignUpRequest, SignUpResult>
    {
        private readonly AppDbContext _context;
        private readonly IHashGenerator _hashGenerator;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPasswordValidator _passwordValidator;
        
        public SignUpOperation(
            ILogger<SignUpOperation> logger,
            AppDbContext context,
            IHashGenerator hashGenerator,
            IDateTimeProvider dateTimeProvider, 
            IHttpContextAccessor httpContextAccessor,
            IPasswordValidator passwordValidator) : base(logger)
        {
            _context = context;
            _hashGenerator = hashGenerator;
            _dateTimeProvider = dateTimeProvider;
            _httpContextAccessor = httpContextAccessor;
            _passwordValidator = passwordValidator;
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
            
            // Validates a given password against a predefined password requirements policy.
            var passwordValidationResult = _passwordValidator.Validate(request.Password);
            if (passwordValidationResult.Passed == false) 
            {
                return ValidationResult.Failure().WithMessage("Unable to sign up - weak password.");
            }

            return ValidationResult.Success();
        }

        protected override async Task<SignUpResult> ExecuteRequest(SignUpRequest request, ValidationResult validationResult)
        {
            // Ensure the database is updated in a transactional manner.
            using var transaction = await _context.Database.BeginTransactionAsync();
            
            // Check if an account with for a given username already exists
            var accountExists = _context.Account.Any(a => a.Username == request.Username);
            if (accountExists) 
            {
                return new SignUpResult { Success = false, Message = "Unable to sign up - account already exists." };
            }

            var now = _dateTimeProvider.GetUtcNow();
            var passwordHash = await _hashGenerator.GenerateHash(request.Password);

            // Create and insert a new account
            await _context.Account.AddAsync(new Data.Entity.Account
            {
                Username = request.Username.Trim(),
                PasswordHash = Encoding.ASCII.GetBytes(passwordHash),
                Created = now,
                CreatedBy = "account-api",
                Modified = now,
                ModifiedBy = "account-api"
            });

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return new SignUpResult { Success = true, Message = "Account registered successfully." };
        }
    }
}