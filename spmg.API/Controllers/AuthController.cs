using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using spmg.API.Data;
using spmg.API.Dtos;
using spmg.API.Models;
using System.IdentityModel.Tokens.Jwt;


namespace spmg.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController:ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;

        public AuthController(IAuthRepository repo, IConfiguration  config)
        {
            _repo = repo;
            _config = config;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register (UserForRegisterDto userForRegisterDto ){
            //validate request
            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();
            if(await _repo.userExists(userForRegisterDto.Username))
                return BadRequest("Usuario Ya Existe");
            
            var userToCreate = new User{
                UserName=userForRegisterDto.Username
            };

            var createdUser = await _repo.Register(userToCreate,userForRegisterDto.Password);

            return StatusCode(201);            
        }

         [HttpPost("Login")]
          public async Task<IActionResult> Login (UserforLoginDto userforLoginDto)
        {
            var userFromRepo = await _repo.Login(userforLoginDto.Username.ToLower(),userforLoginDto.Password  );
            if (userFromRepo== null) 
                return Unauthorized();
           var claims = new[]{
               new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
               new Claim (ClaimTypes.Name,userFromRepo.UserName)
           };

           var key = new SymmetricSecurityKey(Encoding.UTF8
                    .GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor{
                Subject=new ClaimsIdentity(claims),
                Expires=DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok( new {
                token= tokenHandler.WriteToken(token)
            });



        }
    }
}