using System.Linq;
using System.Text;
using System.Threading.Tasks;
using code.Core.Operations;
using code.Data;
using code.Model;
using code.Operations.LogOut;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace code.Core.Application 
{
    public class LogOutOperation : Operation<LogOutRequest, LogOutResponse>
    {
        private readonly AppDbContext _context;
        private readonly IHashGenerator _hashGenerator;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;

        
        public LogOutOperation(
            ILogger<LogOutOperation> logger,
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

        protected override ValidationResult ValidateRequest(LogOutRequest request) 
        {
            var loggedInUser = _httpContextAccessor.HttpContext.Session.GetString(AppConstants.LoggedInUserSessionKey);
            if (string.IsNullOrEmpty(loggedInUser)) 
            {
                return ValidationResult.Failure().WithMessage("Invalid request - already logged out.");
            }
            return ValidationResult.Success();
        }

        protected override Task<LogOutResponse> ExecuteRequest(LogOutRequest request, ValidationResult validationResult)
        {
            // TODO: record last logout time
            _httpContextAccessor.HttpContext.Session.Clear();
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(AppConstants.AppCookieName);
            var logoutResult = new LogOutResponse { Success = true,  Message = "Logged out successfully." };
            return Task.FromResult(logoutResult);
        }
    }
}