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
using TodoApi.helper;

namespace TodoApi.Controllers
{
    [Authorize]
    [ApiController]
    [ServiceFilter(typeof(LogUserActivity))]
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
        public async Task<IActionResult> GetUsers([FromQuery] UserParams userParam){
            userParam.currentUserId=int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
           var users= await _repo.GetUsers(userParam);           
           var userToReturn= _mapper.Map<IEnumerable<UserForListDto>>(users);
           Response.AddPagination(users.PageSize,users.TotalPage,users.CurentPage,users.totalCountOfItems);
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
        [HttpPost("{id}/likes/{recievedId}")]
        public async Task<IActionResult> CreateLike(int id, int recievedId){
            if (id!= int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized("هر کاربر فقط اطلاعات مربوط به خود را باید ویرایش کند");
            }
            var likedUser = await _repo.GetLike(id,recievedId);
            if (likedUser != null)
            {
                return BadRequest("قبلاً این کاربر را پسندیدی!!");
            }
            var like =new Like(){
                LikerId=id,
                LikedId=recievedId
            };
            _repo.Add<Like>(like);
            if (await _repo.SaveAll())
            {
                return Ok();
            }
            return BadRequest("در لایک کردن کاربر انتخابی خطا به وجودآمد");
        }
    }
}