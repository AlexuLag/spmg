using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using spmg.API.Data;
using spmg.API.Dtos;
using spmg.API.Models;

namespace spmg.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController:ControllerBase
    {
        private readonly IAuthRepository _repo;
        public AuthController(IAuthRepository repo)
        {
            _repo = repo;

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
    }
}