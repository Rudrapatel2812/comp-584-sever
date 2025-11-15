using comp_584_sever.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using WorldModel;

namespace comp_584_sever.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController(UserManager<WorldModelUser> userManager, JwtHandler jwtHeandler) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            WorldModelUser? worlduser = await userManager.FindByNameAsync(loginRequest.Username);
            //if (worlduser == null || !await userManager.CheckPasswordAsync(worlduser, loginRequest.Password))
            //{
            //    Response.StatusCode = StatusCodes.Status401Unauthorized;
            //    
            //    return;
            //}
            if (worlduser == null) {
                return Unauthorized("Invalid username");
            }

            bool loginstatus= await userManager.CheckPasswordAsync(worlduser, loginRequest.Password);
            if ( !loginstatus)
            {
                return Unauthorized("Invalid password");
            }
            JwtSecurityToken jwtToken =await jwtHeandler.GenerateTokenAsync(worlduser);
            string stringToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return Ok(new LoginResponse
            {
                Success = true,
                message = "Mom loves me",
                token = stringToken
            });

        }
    }
}
