using Microsoft.EntityFrameworkCore;
using SkillNet.Models.Entities;
using SkillNet.Repositories.Contexts;
using SkillNet.Repositories.Contracts;

namespace SkillNet.Repositories.Implementations
{
    public class User_Repository : IUser_Repository
    {
        private readonly SkillNet_Context _context;
        public User_Repository(SkillNet_Context context) => _context = context;

        public async Task<IEnumerable<User>> GetAllAsync() => await _context.Users.ToListAsync();

        public async Task<User?> GetByIdAsync(int id) => await _context.Users.FindAsync(id);

        public async Task CreateAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}