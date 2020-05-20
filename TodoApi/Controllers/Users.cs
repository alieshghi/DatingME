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
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using System.Collections.Generic;

namespace TodoApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]       
    public class UsersController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IDatingRepository _repo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        public UsersController(ILogger<AuthController> logger,
         IDatingRepository repo,
         IConfiguration config,
         IMapper mapper
         )
        {
            _repo = repo;
            _logger=logger;
            _config=config;
            _mapper=mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetUsers(){
           var users= await _repo.GetUsers();
           var userToReturn= _mapper.Map<IEnumerable<UserForListDto>>(users);
            return Ok(userToReturn);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id){
            var user = await _repo.GetUser(id);
            var userToReturn= _mapper.Map<UserForDetails>(user);
             return Ok(userToReturn);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id,UserForUpdateDto userForUpdateDto){
            if (id!= int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized("هر کاربر فقط اطلاعات مربوط به خود را باید ویرایش کند");
            }
            var userFromServer = await _repo.GetUser(id);
            _mapper.Map(userForUpdateDto,userFromServer);
            if (await _repo.SaveAll())
            {
                return NoContent();
            }
            throw new Exception ($"در ذخیره سازی تغییرات کاربری {id} به مشکل خوردیم");
        }
    }
}