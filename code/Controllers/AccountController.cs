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

namespace code.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly AppDbContext _context;

        public AccountController(ILogger<AccountController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpPost]
        [Route("signup")]
        public IActionResult SignUp(SignUpRequest request) 
        {
            Console.WriteLine($"user: { request.Username }, pass: { request.Password }");
            Console.WriteLine($"Session: { HttpContext.Session }, available{ HttpContext.Session.IsAvailable }");
            var accounts = _context.Account.ToList();
            return Ok();
        }

        [HttpPost]
        [Route("login")]
        public IActionResult LogIn(LogInRequest request) 
        {
            Console.WriteLine($"user: { request.Username }, pass: { request.Password }");
            Console.WriteLine($"Session: { HttpContext.Session }");
            HttpContext.Session.SetString("username", "nkojic@gmail.com");
            return Ok();
        }

        [HttpPost]
        [Route("logout")]
        // [Authorize]
        public IActionResult LogOut() 
        {
            Console.WriteLine($"Logout: { HttpContext.Session.GetString("username") }, Sesid: { HttpContext.Session.Id }");
            
            // Abandon current session
            HttpContext.Session.Clear();
            
            // Invalidate the client cookie
            Response.Cookies.Delete("AppCookie");
            return Ok();
        }
    }
}
