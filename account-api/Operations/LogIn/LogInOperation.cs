using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Docker.Test.Core;
using Docker.Test.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Docker.Test.Operations.LogIn 
{
    public class LogInOperation : Operation<LogInRequest, LogInResult>
    {
        private readonly AppDbContext _context;
        private readonly IHashGenerator _hashGenerator;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        public LogInOperation(
            ILogger<LogInOperation> logger,
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

        protected override ValidationResult ValidateRequest(LogInRequest request) 
        {
            // Sanitize input data
            request.Username = request.Username?.Trim();
            request.Password = request.Password?.Trim();

            if (string.IsNullOrWhiteSpace(request.Username)) 
            {
                return ValidationResult.Failure().WithMessage("Unable to log in - invalid username.");
            }

            if (string.IsNullOrWhiteSpace(request.Password)) 
            {
                return ValidationResult.Failure().WithMessage("Unable to log in - invalid password.");
            }

            return ValidationResult.Success();
        }

        protected override async Task<LogInResult> ExecuteRequest(LogInRequest request, ValidationResult validationResult)
        {
            // Check if the user is already logged ins
            var loggedInUser = _httpContextAccessor.HttpContext.Session.GetString(AppConstants.LoggedInUserSessionKey);
            if (string.Equals(request.Username, loggedInUser, StringComparison.OrdinalIgnoreCase)) 
            {
                return new LogInResult { Success = true, Message = "Already logged in." };
            }

            // Ensure there is an account for a given username
            var account = await _context.Account.FirstOrDefaultAsync(a => a.Username == request.Username);
            if (account == null) 
            {
                return new LogInResult { Success = false, Message = "Unabled to log in - account not found." };
            }
            
            // Check password
            var passwordMatch = await _hashGenerator.VerifyHash(
                hash: Encoding.ASCII.GetString(account.PasswordHash), 
                content: request.Password);
            
            if (passwordMatch) 
            {
                _httpContextAccessor.HttpContext.Session.SetString(AppConstants.LoggedInUserSessionKey, request.Username);
                return new LogInResult { Success = true, Message = "Logged in successfully." };
            }
            else 
            {
                return new LogInResult { Success = false, Message = "Unable to log in - wrong password." };
            }
        }
    }
}