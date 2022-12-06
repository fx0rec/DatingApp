using API.Data;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseAPIController
    {
        
        private readonly IUserRepository _userRepository;
       
        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;

        }
        
        //End-point
        //Has to have the HttpGet attribute used to make the request
        [HttpGet] // This + [Route] makes up our Route
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            return Ok(await _userRepository.GetUsersAsync());
        }

        
        [HttpGet("{username}")]
        //Async
        public async Task<ActionResult<AppUser>> GetUser(string username)
        {
           return Ok(await _userRepository.GetUserByUsernameAsync(username));
        }


    }
}