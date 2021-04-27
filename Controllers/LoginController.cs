using BlazorSimpleApplications.Data;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlazorSimpleApplications.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        [HttpPost]
        public IActionResult Login([FromBody] User user)
        {
            TokenClass tokenClass = new TokenClass();
            User findedUser = new UserRepository().GetUser(user.UserName);
            if (findedUser == null)
            {
                tokenClass.TokenOrMessage = "Unauthorized User";
                return Ok(tokenClass);

            }

            bool credentials = findedUser.Password.Equals(user.Password);
            if (!credentials)
            {
                tokenClass.TokenOrMessage = "Invalid Password";
                return Ok(tokenClass);
            }
            tokenClass.TokenOrMessage = TokenManager.GenerateToken(user.UserName);
            return Ok(tokenClass);

        }
    }
}
