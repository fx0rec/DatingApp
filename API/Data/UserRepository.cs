
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        public UserRepository(DataContext context)
        {
            _context = context;
            
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