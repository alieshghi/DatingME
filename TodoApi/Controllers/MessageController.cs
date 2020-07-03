using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Data;
using TodoApi.Dtos;
using TodoApi.helper;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Authorize]
    [ApiController]
    [ServiceFilter(typeof(LogUserActivity))]
    [Route("Users/{Userid}/[controller]")] 
    public class MessageController: ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;
        public MessageController(IDatingRepository repo, IMapper mapper )
        {
            _repo = repo;
            _mapper = mapper;
        }
        [HttpGet("{id}", Name = "GetMessage")]
        public async Task<IActionResult> GetMessage(int userId, int id){
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            var message= await _repo.GetMessageById(id);
            if (message==null)
            {
                return NotFound();
            }
            return Ok(message);
        }
        [HttpGet]
        public async Task<IActionResult> GetMessagesForUser(int userId,[FromQuery] MessageParams messageParams){
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
                messageParams.UserId = userId;
            var message= await _repo.GetMessages(messageParams);
            var messageDto= _mapper.Map<IEnumerable<MessageToReturnDto>>(message);
            Response.AddPagination(message.PageSize,message.TotalPage,message.CurentPage,message.totalCountOfItems);
            return Ok(messageDto);
        }
        [HttpPost]
        public async Task<IActionResult> CreateMessage(int userId,MessageToCreateDto messageToCreateDto){
            var sender = await _repo.GetUser(userId);
            if (sender.Id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
                if (messageToCreateDto == null)
                {
                    return BadRequest();
                }

            var recipient = await _repo.GetUser(messageToCreateDto.RecipientId);

            if (recipient == null)
                return BadRequest("Could not find recipient user");            
            messageToCreateDto.SenderId = userId;
            var message =_mapper.Map<Message>(messageToCreateDto);
            _repo.Add<Message>(message);
            if (await _repo.SaveAll())
                {
                   var messageToReturn = _mapper.Map<MessageToReturnDto>(message);
                   return CreatedAtAction("GetMessage",new {controller = "Message" ,id = message.Id},messageToReturn);
                }
                throw new Exception("Creating the message failed on save");
        }
        [HttpPost("{id}") ]
        public async Task<IActionResult> DeleteMessage(int id, int userId){
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            var message = await _repo.GetMessageById(id);
            if (message == null)
            {
                return BadRequest(" message is not exists");
            }
            if (message.SenderId == userId)
            {
                message.SenderDeleted = true;
            }
            if (message.RecipientId == userId )
            {
                message.RecipientDeleted = true;
            }
            if (message.RecipientDeleted && message.SenderDeleted)
            {
                _repo.Delete(message);
            }
            if (await _repo.SaveAll())
            {
                return NoContent();
            }
            throw new Exception("can not delete message");
        }
        [HttpGet("thread/{recipientId}")]
        public async Task<IActionResult> GetMessageThread(int userId, int recipientId){
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                    return Unauthorized();
            var recipient = await _repo.GetUser(recipientId);

            if (recipient == null)
                return BadRequest("Could not find recipient user");
            var message = await _repo.GetMessageThread(userId,recipientId);
            var messageToReturn = _mapper.Map<IEnumerable<MessageToReturnDto>>(message);
            return Ok(messageToReturn);
        }
        [HttpPost("{id}/read")]
        public async Task<IActionResult> MarkAsRead(int userId, int id){
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                    return Unauthorized();
            var message = await _repo.GetMessageById(id);
            if (message == null)
            {
                return BadRequest("Could not find message");
            }
            if (message.RecipientId!=userId)
            {
                return Unauthorized();
            }
            message.IsRead = true;
            message.DateRead = DateTime.Now;
            if (await _repo.SaveAll())
            {
                return NoContent();
            }
            throw new Exception("can not markAsRead");
        }


    }
}