using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController ]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly ITokenHandler tokenHandler;

        public AuthController(IUserRepository userRepository ,ITokenHandler tokenHandler )
        {
            
            this.userRepository = userRepository;
            this.tokenHandler = tokenHandler;
             
        }

     //   public ITokenHandler TokenHandler { get; }

        [HttpPost]
        [Route("login")]
        public async  Task<IActionResult>  LoginAsync(Models.DTO.LoginRequest loginRequest)
        {  //Validate incoming request [implement fluent]

            //check if user authenticated
            //check user name and password
          var user= await userRepository.AuthenticateAsync(
              loginRequest.username, loginRequest.password);

            if(user!=null )
            {
                //generate a Jwt Token
             var token= await tokenHandler.CreateTokenAsync(user);
                return Ok(token);
            }
            return BadRequest("User Name or password incorrect");
        }
    }
}
