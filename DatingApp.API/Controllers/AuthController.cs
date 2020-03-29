using Microsoft.AspNetCore.Mvc;
using DatingApp.API.Data;
using System.Threading.Tasks;
using DatingApp.API.Models;
using DatingApp.API.Dtos;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace DatingApp.API.Controllers
{

    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;

        private IConfiguration _config;

        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }

     [HttpPost("register")]
     public async Task<IActionResult> Register(UserRegisterDto userRegisterDto)
     {        
         userRegisterDto.Username = userRegisterDto.Username.ToLower();

         if(await _repo.UserExist(userRegisterDto.Username))        
           return BadRequest("username already exist");

            var userToCreate = new User
            {
                Username = userRegisterDto.Username
            } ;        

           var createdUser = await _repo.Register(userToCreate, userRegisterDto.Password);

           return StatusCode(201); 
      }      
        
      
        

     
     [HttpPost("login")]
     public async Task<IActionResult> Login(LoginRegsiterDto loginRegisterDto)
     {        
         //throw new Exception("Computer says no!!!");
        var userFromRepo = await _repo.Login(loginRegisterDto.Username, loginRegisterDto.Password);

            if(userFromRepo == null)            
               return Unauthorized();    

        var claims = new []
        {
            new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
            new Claim(ClaimTypes.Name, userFromRepo.Username)
        };

        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(1),
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);
            
            return Ok(new 
            {
                token = tokenHandler.WriteToken(token)
            });
     } 
    }
       

       
}