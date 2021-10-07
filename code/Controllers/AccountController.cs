using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using code.Model;
using Microsoft.AspNetCore.Authorization;
using code.Data;
using code.Core;
using code.Core.Operations;
using code.Core.Application;
using code.Operations.LogOut;

namespace code.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly AppDbContext _context;
        private readonly IOperationProvider _appOperationProvider;

        public AccountController(
            ILogger<AccountController> logger, 
            AppDbContext context,
            IOperationProvider operationProvider)
        {
            _logger = logger;
            _context = context;
            _appOperationProvider = operationProvider;
        }

        [HttpPost]
        [Route("signup")]
        public async Task<IActionResult> SignUp(SignUpRequest request) 
        {
            var signUpOperation = _appOperationProvider.GetOperation<SignUpOperation>();
            var result = await signUpOperation.Execute(request);
            return Ok(result);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LogIn(LogInRequest request) 
        {
            var logInOperation = _appOperationProvider.GetOperation<LogInOperation>();
            var result = await logInOperation.Execute(request);
            return Ok(result);
        }

        [HttpPost]
        [Route("logout")]
        public IActionResult LogOut()
        {
            var logOutRequest = new LogOutRequest();
            var logOutOperation = _appOperationProvider.GetOperation<LogOutOperation>();
            var result = logOutOperation.Execute(logOutRequest);
            return Ok(result);
        }
    }
}
