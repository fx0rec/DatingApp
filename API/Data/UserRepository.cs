
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public UserRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
            
        }

        public async Task<MemberDto> GetMemberAsync(string username)
        {
            //Manual way
            /*
            return await _context.Users
            .Where(x=>x.UserName == username)
            .Select(user => new MemberDto
            {
                Id = user.Id,
                UserName = user.UserName,
                KnownAs = user.KnownAs,

            }).SingleOrDefaultAsync();*/

            //Automapper way

            return await _context.Users
            .Where(x=>x.UserName == username)
            .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<MemberDto>> GetMembersAsync()
        {
            return await _context.Users
            .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.
            Include(p => p.Photos). //Includes photos
            FirstOrDefaultAsync(x => x.UserName == username); // x=> x.UserName == username clarifies that AppUser.UserName == username 
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _context.Users.
            Include(p => p.Photos).
            ToListAsync(); //Returns a list of all users + photos as included in the request
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0; //Save changes if changes are greater than 0
        }

        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
            //This tells our Entity framework tracker that an entity has been updated.
            //Changes aren't saved in this stage. 
            //The need of this is debated, as Entity will know the changes. But it's included just in case
        }
    }
}