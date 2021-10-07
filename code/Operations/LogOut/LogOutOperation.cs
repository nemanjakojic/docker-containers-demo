using System.Linq;
using System.Text;
using System.Threading.Tasks;
using code.Core.Operations;
using code.Data;
using code.Model;
using code.Operations.LogOut;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace code.Core.Application {

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

        protected override Task<ValidationResult> ValidateRequest(LogOutRequest request) {
            return Task.FromResult(ValidationResult.Success());
        }

        protected override Task<LogOutResponse> ExecuteRequest(LogOutRequest request, ValidationResult validationResult)
        {
            _httpContextAccessor.HttpContext.Session.Clear();
            _httpContextAccessor.HttpContext.Response.Cookies.Delete("AppCookie");
            return Task.FromResult(new LogOutResponse { Success = true });
        }
    }
}