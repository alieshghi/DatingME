using System.Threading;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Net;
using System.Security.Claims;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TodoApi.Data;
using TodoApi.Dtos;
using TodoApi.helper;
using TodoApi.Models;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;

namespace TodoApi.Controllers
{
    [Authorize]
    [Route("users/{userId}/photos")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IOptions<CloudinarySetting> _cloudinaryConfig;
        private readonly IMapper _mapper;
        private  Cloudinary _cloudinary;
        private readonly ILogger<PhotosController> _logger;

        public PhotosController(IDatingRepository repo,
        IOptions<CloudinarySetting> cloudinaryConfig,
        IMapper mapper,
        ILogger<PhotosController> logger)
        {
            _mapper = mapper;
            _cloudinaryConfig = cloudinaryConfig;
            _repo = repo;
            _logger = logger;
            Account account=new Account(
                _cloudinaryConfig.Value.Cloudname
                ,_cloudinaryConfig.Value.APIKey
                ,_cloudinaryConfig.Value.APISecret);
                _cloudinary =new Cloudinary(account);
        }
        [HttpGet("{id}",Name="GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id){
            var photoToShow = await _repo.GetPhoto(id);
            var result = _mapper.Map<PhotoToReturnDto>(photoToShow);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> CreateUserPhotos(int userId,[FromForm]PhotosToCreatDto photosTocreatDto)
        {
            if (userId!= int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized("هر کاربر فقط اطلاعات مربوط به خود را باید ویرایش کند");
            }
            var userFromServer = await _repo.GetUser(userId);
            var file= photosTocreatDto.File;
            var uploadResult = new ImageUploadResult();
            if (file.Length>0)
            {
                using (var stream= file.OpenReadStream() )
                {
                    var updateParamas= new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name,stream),
                        Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
                        
                    };
                    try
                    {
                        uploadResult = _cloudinary.Upload(updateParamas);
                    }
                    catch (Exception ex)
                    {
                        
                        _logger.LogError(ex.Message); 
                        return BadRequest(ex);
                    }                    
                
                }
                
            }
            photosTocreatDto.Url = uploadResult.Uri.ToString();
            photosTocreatDto.publicID = uploadResult.PublicId;
            var photo = _mapper.Map<Photo>(photosTocreatDto);
            if (!userFromServer.Photos.Any(x => x.IsMain))
            {
                photo.IsMain=true;
            }
            userFromServer.Photos.Add(photo);
            
            if (await _repo.SaveAll())
            {
                var createdPhoto= _mapper.Map<PhotoToReturnDto>(photo);
                return CreatedAtAction("GetPhoto",new{id= photo.Id},createdPhoto);
            }
            return BadRequest("امکان ذخیره سازی وجود ندارد");
        }
        [HttpPost("{id}/setMain")]
        public async Task<IActionResult> SetMain(int userId,int id){
            if (userId!= int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized("هر کاربر فقط اطلاعات مربوط به خود را باید ویرایش کند");
            }          
            var photoFromServer= await _repo.GetPhoto(id);
            if (photoFromServer.UserId!=userId)
            {
                return BadRequest("هر کاربر فقط اطلاعات مربوط به خود را باید ویرایش کند");
            }
            if (photoFromServer.IsMain)
            {
                return BadRequest("پیشتر این عکس برای پروفایل انتخاب شده است");
            }
            var mainPhotoFromServer= await _repo.GetCurrentMainPhoto(userId);            
            mainPhotoFromServer.IsMain=false;
            photoFromServer.IsMain=true;
            if (await _repo.SaveAll())
            {
                return NoContent();
            }
            return BadRequest("در ذخیره سازی اطلاعات به مشکل خوردیم");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(int userId,int id){
            if (userId!= int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized("هر کاربر فقط اطلاعات مربوط به خود را باید حذف کند");
            }          
            var photoFromServer= await _repo.GetPhoto(id);
            if (photoFromServer.UserId!=userId)
            {
                return BadRequest("هر کاربر فقط اطلاعات مربوط به خود را باید حذف کند");
            }
            if (photoFromServer.IsMain)
            {
                return BadRequest("امکان حذف پروفایل وجود ندارد");
            }
            if (!string.IsNullOrEmpty(photoFromServer.PublicId) )
            {
                var deleteParams=new DeletionParams(photoFromServer.PublicId);
                var result= _cloudinary.Destroy(deleteParams);                
            }
            _repo.Delete(photoFromServer);
            if (await _repo.SaveAll())
            {
                return Ok();
            }
            return BadRequest("عملیات حذف با شکست مواجه شد");

        }



    }
}