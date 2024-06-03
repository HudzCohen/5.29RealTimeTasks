using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealTimeTasks.Data;
using RealTimeTasks.Web.Models;
using System.Security.Claims;

namespace RealTimeTasks.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly string _connectionString;

        public AccountController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }

        [HttpPost("signup")]
        public void Signup(SignupViewModel user)
        {
            var repo = new UserRepo(_connectionString);
            repo.AddUser(user, user.Password);
        }

        [HttpPost("login")]
        public User Login(LoginViewModel loginViewModel)
        {
            var repo = new UserRepo(_connectionString);
            var user = repo.Login(loginViewModel.Email, loginViewModel.Password);
            if(user == null)
            {
                return null;
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, loginViewModel.Email)
            };

            HttpContext.SignInAsync(new ClaimsPrincipal(
                new ClaimsIdentity(claims, "Cookies", ClaimTypes.Email, "role"))).Wait();

            return user;
        }

        [HttpGet("getcurrentuser")]
        public User GetCurrentUser()
        {
            if(!User.Identity.IsAuthenticated)
            {
                return null;
            }

            var repo = new UserRepo(_connectionString);
            return repo.GetByEmail(User.Identity.Name);
        }

        [HttpPost("logout")]
        public void Logout()
        {
            HttpContext.SignOutAsync().Wait();
        }
    }
}
