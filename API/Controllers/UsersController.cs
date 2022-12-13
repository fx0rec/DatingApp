using System.Security.Claims;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseAPIController
    {
        
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
       
        public UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService) //Injecting IUserRepository + Automapper + IPhotoService
        {
            _photoService = photoService;
            _mapper = mapper;
            _userRepository = userRepository;

        }
        
        //End-point
        //Has to have the HttpGet attribute used to make the request
        [HttpGet] // This + [Route] makes up our Route
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            var users = await _userRepository.GetMembersAsync();
            return Ok(users);
        }

        
        [HttpGet("{username}")]
        //Async
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
           return await _userRepository.GetMemberAsync(username);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto){
             
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            if(user == null) return NotFound();

            _mapper.Map(memberUpdateDto, user);

            if(await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to update user");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            if(user == null) return NotFound();

            var result = await _photoService.AddPhotoAsync(file); 

            if(result.Error != null) return BadRequest(result.Error.Message); //If there is an error, return error message in a BadRequest()

            var photo = new Photo 
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            if (user.Photos.Count == 0) photo.IsMain = true; // If this is the first photo, set to main
            user.Photos.Add(photo);
                                            //If there are changes saved into our DB, then use mapper to map into PhotoDto from photo
            if(await _userRepository.SaveAllAsync()) 
            {                                              //Passing user as an object
                return CreatedAtAction(nameof(GetUser), new {username = user.UserName}, _mapper.Map<PhotoDto>(photo)); //Returns 201 created response
            }

            //if not, return a bad request, something has gone wrong.
            return BadRequest("Problem adding photo.");

        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            //Get the user information
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
            if(user == null) return NotFound("User not found");


            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo == null) return NotFound("Photo not found");

            if(photo.IsMain) return BadRequest("This is already your main photo");

            //Getting the current main photo
            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
            //If currentMain != null, we have a main set to true, then change it to false
            if (currentMain != null) currentMain.IsMain = false;
            //Then change the current photo we're updating isMain = true
            photo.IsMain = true;

            //If updated, save changes and return 204 No Content
            if (await _userRepository.SaveAllAsync()) return NoContent();
            //If that fails, return a bad request.
            return BadRequest("Problem setting main photo");
        }
    }
}