using System;
using System.Text;
using System.Security.Cryptography;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TodoApi.Data;
using TodoApi.Dtos;
using TodoApi.Models;
using Microsoft.IdentityModel.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using AutoMapper;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("[controller]")]       
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        public AuthController(ILogger<AuthController> logger,
         IAuthRepository repo,
         IConfiguration config,
         IMapper mapper
         )
        {
            _repo = repo;
            _logger=logger;
            _config=config;
            _mapper=mapper;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userDto){
            userDto.UserName= userDto.UserName.ToLower();
            if (userDto.Password.Length<4)
            {
                return BadRequest("تعداد کاراکترهای پسورد باید بزرگتر از 4 باشد");
            }
            if ( await _repo.UserExists(userDto.UserName))
            {
                return BadRequest("این حساب کاربری قبلاً ثبت شده است");
            }
            var newUser = new User{
                UserName=userDto.UserName
            };
            var createdUser= await _repo.Register(newUser,userDto.Password);
            return StatusCode(201);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userDto){
            var user = await _repo.Login(userDto.UserName.ToLower(),userDto.Password);
             if (user==null)
             {
                  return Unauthorized("کاربری یا کلمه عبور صحیح نیست");
             }
             var claims =new []{
                 new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                 new Claim(ClaimTypes.Name,user.UserName)
             };

             var key=new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_config.GetSection("AppSettings:Token").Value));
             var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
             var tokenDescriptor= new SecurityTokenDescriptor {
                 Subject = new ClaimsIdentity(claims),
                 Expires = DateTime.Now.AddMinutes(20),
                 SigningCredentials=creds
             };

             var tokenHandler= new JwtSecurityTokenHandler();
             var token =tokenHandler.CreateToken(tokenDescriptor);

             return Ok(new{
               token = tokenHandler.WriteToken(token)   
             });
        }
    }
}