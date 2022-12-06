using API.Data;
using API.DTOs;
using API.Entities;
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
       
        public UsersController(IUserRepository userRepository, IMapper mapper) //Injecting IUserRepository + Automapper
        {
            _mapper = mapper;
            _userRepository = userRepository;

        }
        
        //End-point
        //Has to have the HttpGet attribute used to make the request
        [HttpGet] // This + [Route] makes up our Route
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            var users = await _userRepository.GetUsersAsync();
            var usersToReturn = _mapper.Map<IEnumerable<MemberDto>>(users);

            return Ok(usersToReturn);
        }

        
        [HttpGet("{username}")]
        //Async
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
           var user = await _userRepository.GetUserByUsernameAsync(username);

           return _mapper.Map<MemberDto>(user);
        }


    }
}