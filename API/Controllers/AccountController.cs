using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace API.Controllers
{
    public class AccountController : BaseAPIController
    {

        //Dependency Injection [DI]
        private readonly DataContext _context;  //Initialize field from parameter (lightbulb)

        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(DataContext context, ITokenService tokenService, IMapper mapper)
        {
            _mapper = mapper;
            _tokenService = tokenService;

            _context = context; //initialized from field parameter

        }

        // /api/ is the folder, then /account/ is the first word in this class.
        // This method is accepting two parameters, but has no idea how to send it.
        // Hence why we need to use the following query: ?username=dave&password=pwd
        [HttpPost("register")] // POST: api/account/register?username=dave&password=pwd
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.Username)) return BadRequest("Username is taken.");

            var user = _mapper.Map<AppUser>(registerDto);

            //[using] keyword specifies that we want this object removed from the heap after use
            // through the Dispose() method inside the base class of HMACSHA512
            using var hmac = new HMACSHA512(); // Generates a random key that's gonna be used as Pw Salt

            user.UserName = registerDto.Username.ToLower();
            //  ComputeHash()
            //  Computes the hash value for the specified byte array.
            //  Returns: The computed hash code. 

            //  GetBytes()
            //  When overridden in a derived class, encodes all the characters in the
            //  specified string into a sequence of bytes.
            //  Returns:A byte array containing the results of encoding the specified set of characters.
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
            user.PasswordSalt = hmac.Key;


            _context.Users.Add(user);
            //Things don't get added until SaveChangesAsync() is called!
            await _context.SaveChangesAsync();

            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user),
                KnownAs = user.KnownAs
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            //SingleOrDefaultAsync() || FirstOrDefaultAsync() either works fine.
            //If database has more than one entry with that username
            //It will throw an exception
            var user = await _context.Users
            .Include(p => p.Photos)  //We must include photos, else it wont load the photo in the navbar
            .SingleOrDefaultAsync(x =>
            x.UserName == loginDto.Username);

            if (user == null) return Unauthorized("Invalid username.");

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password.");
            }

            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                KnownAs = user.KnownAs
            };

        }

        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
    }

}
