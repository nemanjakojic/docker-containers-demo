using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Array.Test.Core;
using Array.Test.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Array.Test.Operations.LogOut 
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

        protected override Task<LogOutResponse> ExecuteRequest(LogOutRequest request, ValidationResult validationResult)
        {
            // TODO: record last logout time (optionally)
            
            // Clears the active session (removes all the values from it)
            _httpContextAccessor.HttpContext.Session.Clear();
            
            // Deletes the session token from the client
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(AppConstants.AppCookieName);
            
            var logoutResult = new LogOutResponse { Success = true,  Message = "Logged out successfully." };
            return Task.FromResult(logoutResult);
        }
    }
}