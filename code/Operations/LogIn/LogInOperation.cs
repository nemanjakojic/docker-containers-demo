using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using code.Core.Operations;
using code.Data;
using code.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace code.Core.Application 
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

        protected override Task<ValidationResult> ValidateRequest(LogInRequest request) 
        {
            var loggedInUser = _httpContextAccessor.HttpContext.Session.GetString(AppConstants.LoggedInUserSessionKey);

            if (string.Equals(request.Username, loggedInUser, StringComparison.OrdinalIgnoreCase)) 
            {
                return Task.FromResult(ValidationResult.Failure().WithMessage("Already logged in."));
            }

            // Sanitize input data
            request.Username = request.Username?.Trim();
            request.Password = request.Password?.Trim();

            if (string.IsNullOrWhiteSpace(request.Username)) 
            {
                return Task.FromResult(ValidationResult.Success().WithMessage("Invalid username."));
            }

            if (string.IsNullOrWhiteSpace(request.Password)) 
            {
                return Task.FromResult(ValidationResult.Success().WithMessage("Invalid password."));
            }
            
            return Task.FromResult(ValidationResult.Success());
        }

        protected override async Task<LogInResult> ExecuteRequest(LogInRequest request, ValidationResult validationResult)
        {
            var account = await _context.Account.Where(a => a.Username == request.Username).FirstOrDefaultAsync();
            if (account == null) 
            {
                return new LogInResult { Success = false, Message = "Account not found." };
            }
            
            var passwordMatch = await _hashGenerator.VerifyHash(
                hash: Encoding.ASCII.GetString(account.PasswordHash), 
                content: request.Password);
            
            if (passwordMatch) 
            {
                _httpContextAccessor.HttpContext.Session.SetString(AppConstants.LoggedInUserSessionKey, request.Username);
            }

            return new LogInResult { Success = passwordMatch };
        }
    }
}